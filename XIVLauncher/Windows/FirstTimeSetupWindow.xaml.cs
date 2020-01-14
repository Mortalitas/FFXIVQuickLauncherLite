using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using XIVLauncher.Game;
using XIVLauncher.Settings;

namespace XIVLauncher.Windows
{
    /// <summary>
    ///     Interaction logic for FirstTimeSetup.xaml
    /// </summary>
    public partial class FirstTimeSetup : Window
    {
        public LauncherSettings Result;

        public FirstTimeSetup(LauncherSettings setting)
        {
            InitializeComponent();

            Result = setting;

            var detectedPath = Util.TryGamePaths();

            if (detectedPath != null) GamePathEntry.Text = detectedPath;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SetupTabControl.SelectedIndex == 0)
                if (!Util.IsValidFfxivPath(GamePathEntry.Text))
                {
                    MessageBox.Show("The path you selected is not a valid FFXIV installation", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            if (SetupTabControl.SelectedIndex == 1)
            {
                Result.GamePath = new DirectoryInfo(GamePathEntry.Text);
                Result.IsDx11 = Dx11RadioButton.IsChecked == true;
                Result.Language = (ClientLanguage) LanguageComboBox.SelectedIndex;
                Result.SteamIntegrationEnabled = SteamCheckBox.IsChecked == true;

                Result.Save();
                Close();
            }

            SetupTabControl.SelectedIndex++;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Dx9DisclaimerTextBlock.Visibility = Visibility.Visible;
        }

        private void Dx9RadioButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Dx9DisclaimerTextBlock.Visibility = Visibility.Hidden;
        }
    }
}