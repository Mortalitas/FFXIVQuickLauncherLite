using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XIVLauncher.Game;

namespace XIVLauncher.Windows
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            GamePathEntry.Text = Settings.GamePath.FullName;

            if (Settings.IsDX11())
                Dx11RadioButton.IsChecked = true;
            else
                Dx9RadioButton.IsChecked = true;

            LanguageComboBox.SelectedIndex = (int) Settings.GetLanguage();

            SteamIntegrationCheckBox.IsChecked = Settings.SteamIntegrationEnabled;

            LaunchArgsTextBox.Text = Settings.AdditionalLaunchArgs;

            VersionLabel.Text += " - v" + Util.GetAssemblyVersion() + " - " + Util.GetGitHash();
        }

        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.GamePath = new DirectoryInfo(GamePathEntry.Text);
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
            Process.Start(Path.Combine(GamePathEntry.Text, "boot", "ffxivboot.exe"));
        }

        private void DiscordButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/39WpvU2");
        }
    }
}