﻿using System;
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
            ExceptionTextBox.AppendText("\n" + Environment.OSVersion);
            ExceptionTextBox.AppendText("\n" + Environment.Is64BitProcess);
            ExceptionTextBox.AppendText("\n" + setting.IsDx11);
            ExceptionTextBox.AppendText("\n" + setting.AutologinEnabled);
            ExceptionTextBox.AppendText("\n" + setting.AutologinEnabled);
            ExceptionTextBox.AppendText("\n" + setting.Language);

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
