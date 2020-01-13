using System;
using System.IO;
using System.Windows;
using Serilog;
using XIVLauncher.Windows;

namespace XIVLauncher
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            var release = $"xivlauncher-{Util.GetAssemblyVersion()}-{Util.GetGitHash()}";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(a =>
                    a.File(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "XIVLauncherLite", "output.log")))
#if DEBUG
                .WriteTo.Debug()
                .MinimumLevel.Verbose()
#endif
                .CreateLogger();

            Log.Information(
                $"XIVLauncherLite started as {release}");
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Check if dark mode is enabled on windows, if yes, load the dark theme
            var themeUri =
                new Uri(
                    "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml",
                    UriKind.RelativeOrAbsolute);
            if (Util.IsWindowsDarkModeEnabled())
                themeUri = new Uri(
                    "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml",
                    UriKind.RelativeOrAbsolute);

            Current.Resources.MergedDictionaries.Add(new ResourceDictionary {Source = themeUri});
            Log.Information("Loaded UI theme resource.");

#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                new ErrorWindow((Exception) args.ExceptionObject, "An unhandled exception occured.", "Unhandled")
                    .ShowDialog();
                Log.CloseAndFlush();
                Environment.Exit(0);
            };
#endif

            // Check if the accountName parameter is provided, if yes, pass it to MainWindow
            var accountName = "";

            if (e.Args.Length > 0 && e.Args[0].StartsWith("--account="))
                accountName = e.Args[0].Substring(e.Args[0].IndexOf("=", StringComparison.InvariantCulture) + 1);
            
            Log.Information("Loading MainWindow for account '{0}'", accountName);

            var mainWindow = new MainWindow(accountName);
        }
    }
}