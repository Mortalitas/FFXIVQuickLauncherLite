using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using XIVLauncher.Game;
using XIVLauncher.Settings;

namespace XIVLauncher.Windows
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : INotifyPropertyChanged
    {
        private string gamePath;

        /// <summary>
        /// Gets or sets the path to the game folder.
        /// </summary>
        public string GamePath
        {
            get => gamePath;
            set
            {
                gamePath = value;
                OnPropertyChanged(nameof(GamePath));
            }
        }

        private LauncherSettings _setting;

        public SettingsWindow(LauncherSettings setting)
        {
            InitializeComponent();

            _setting = setting;

            DataContext = this;
            GamePath = _setting.GamePath?.FullName;

            if (_setting.IsDx11)
                Dx11RadioButton.IsChecked = true;
            else
            {
                Dx9RadioButton.IsChecked = true;
                Dx9DisclaimerTextBlock.Visibility = Visibility.Visible;
            }

            LanguageComboBox.SelectedIndex = (int) _setting.Language;

            SteamIntegrationCheckBox.IsChecked = _setting.SteamIntegrationEnabled;

            LaunchArgsTextBox.Text = _setting.AdditionalLaunchArgs;

            VersionLabel.Text += " - v" + Util.GetAssemblyVersion() + " - " + Util.GetGitHash() + " - " + Environment.Version;
        }

        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _setting.GamePath = !string.IsNullOrEmpty(GamePath) ? new DirectoryInfo(GamePath) : null;
            _setting.IsDx11 = Dx11RadioButton.IsChecked == true;
            _setting.Language = (ClientLanguage) LanguageComboBox.SelectedIndex;

            _setting.SteamIntegrationEnabled = SteamIntegrationCheckBox.IsChecked == true;

            _setting.AdditionalLaunchArgs = LaunchArgsTextBox.Text;

            _setting.Save();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Mortalitas/FFXIVQuickLauncherLite");
        }

        private void OriginalLauncherButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isSteam =
                MessageBox.Show("Launch as a steam user?", "XIVLauncherLite", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes;
            _setting.StartOfficialLauncher(isSteam);
        }

        private void DiscordButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/39WpvU2");
        }

        private void Dx9RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Dx9DisclaimerTextBlock.Visibility = Visibility.Visible;
        }

        private void Dx9RadioButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Dx9DisclaimerTextBlock.Visibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}