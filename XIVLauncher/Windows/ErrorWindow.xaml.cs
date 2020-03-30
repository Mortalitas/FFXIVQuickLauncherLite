using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Documents;
using XIVLauncher.Settings;

namespace XIVLauncher.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception exc, string message, string context)
        {
            InitializeComponent();

            var setting = LauncherSettings.Load();

            ExceptionTextBox.AppendText(exc.ToString());
            ExceptionTextBox.AppendText("\n" + Util.GetAssemblyVersion());
            ExceptionTextBox.AppendText("\n" + Util.GetGitHash());
            ExceptionTextBox.AppendText("\nContext: " + context);
            ExceptionTextBox.AppendText("\nOS: " + Environment.OSVersion);
            ExceptionTextBox.AppendText("\n" + Environment.Is64BitProcess);

            if (setting != null)
            {
                ExceptionTextBox.AppendText("\nDX11? " + setting.IsDx11);
                ExceptionTextBox.AppendText("\nAuto Login Enabled? " + setting.AutologinEnabled);
                ExceptionTextBox.AppendText("\nLanguage: " + setting.Language);
                ExceptionTextBox.AppendText("\nGame path: " + setting.GamePath);

                // When this happens we probably don't want them to run into it again, in case it's an issue with a moved game for example
                setting.AutologinEnabled = false;
            }

#if DEBUG
            ExceptionTextBox.AppendText("\nDebugging");
#endif

            ContextTextBlock.Text = message;

            Serilog.Log.Error("ErrorWindow called: [{0}] [{1}]\n" + new TextRange(ExceptionTextBox.Document.ContentStart, ExceptionTextBox.Document.ContentEnd).Text, message, context);

            SystemSounds.Hand.Play();

            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DiscordButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/39WpvU2");
        }
    }
}
