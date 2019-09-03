using System;
using System.Windows;
using XIVLauncher;
using XIVLauncher.Accounts;

namespace XIVLauncher.Windows
{
    /// <summary>
    /// Interaction logic for FirstTimeSetup.xaml
    /// </summary>
    public partial class ProfilePictureInputWindow : Window
    {
        public string ResultName, ResultWorld;

        public ProfilePictureInputWindow(XivAccount account)
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ResultName = CharacterNameTextBox.Text;
            ResultWorld = WorldNameTextBox.Text;

            Close();
        }
    }
}
