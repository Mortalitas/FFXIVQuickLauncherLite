using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using XIVLauncher.Game;

namespace XIVLauncher.Windows
{
    /// <summary>
    ///     Interaction logic for FirstTimeSetup.xaml
    /// </summary>
    public partial class FirstTimeSetup : Window
    {
        public FirstTimeSetup()
        {
            InitializeComponent();

            var detectedPath = Util.TryGamePaths();

            if (detectedPath != null) GamePathEntry.Text = detectedPath;
        }

        private string FindAct()
        {
            try
            {
                var parentKey =
                    Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");

                var nameList = parentKey.GetSubKeyNames();
                foreach (var name in nameList)
                {
                    var regKey = parentKey.OpenSubKey(name);

                    var value = regKey.GetValue("DisplayName");
                    if (value != null && value.ToString() == "Advanced Combat Tracker (remove only)")
                        return Path.GetDirectoryName(regKey.GetValue("UninstallString").ToString()
                            .Replace("\"", string.Empty));
                }

                return null;
            }
            catch
            {
                return null;
            }
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
                Settings.SetGamePath(GamePathEntry.Text);
                Settings.SetDx11(Dx11RadioButton.IsChecked == true);
                Settings.SetLanguage((ClientLanguage) LanguageComboBox.SelectedIndex);

                Settings.Save();
                Close();
            }

            SetupTabControl.SelectedIndex++;
        }
    }
}