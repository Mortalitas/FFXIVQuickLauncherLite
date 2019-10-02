using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XIVLauncher.Game;

namespace XIVLauncher.Windows
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : INotifyPropertyChanged
    {
        private string gamePath;

        /// <summary>
        /// Gets a value indicating whether the "Run Integrity Checks" button is enabled.
        /// </summary>
        public bool IsRunIntegrityCheckPossible =>
            !string.IsNullOrEmpty(GamePath) && Directory.Exists(GamePath);

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
                OnPropertyChanged(nameof(IsRunIntegrityCheckPossible));
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
            GamePath = Settings.GamePath?.FullName;

            if (Settings.IsDX11())
                Dx11RadioButton.IsChecked = true;
            else
            {
                Dx9RadioButton.IsChecked = true;
                Dx9DisclaimerTextBlock.Visibility = Visibility.Visible;
            }

            LanguageComboBox.SelectedIndex = (int) Settings.GetLanguage();

            SteamIntegrationCheckBox.IsChecked = Settings.SteamIntegrationEnabled;

            LaunchArgsTextBox.Text = Settings.AdditionalLaunchArgs;

            VersionLabel.Text += " - v" + Util.GetAssemblyVersion() + " - " + Util.GetGitHash() + " - " + Environment.Version;
        }

        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.GamePath = !string.IsNullOrEmpty(GamePath) ? new DirectoryInfo(GamePath) : null;
            Settings.SetDx11(Dx11RadioButton.IsChecked == true);
            Settings.SetLanguage((ClientLanguage) LanguageComboBox.SelectedIndex);

            Settings.SteamIntegrationEnabled = SteamIntegrationCheckBox.IsChecked == true;

            Settings.AdditionalLaunchArgs = LaunchArgsTextBox.Text;

            Settings.Save();
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
            Process.Start(Path.Combine(GamePath, "boot", "ffxivboot.exe"));
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