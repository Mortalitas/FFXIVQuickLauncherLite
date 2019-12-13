using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using XIVLauncher.Game;

namespace XIVLauncher
{
    // TODO: All of this needs a rework
    static class Settings
    {
        public static Action LanguageChanged;

        public static DirectoryInfo GamePath
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.GamePath))
                    return null;

                return new DirectoryInfo(Properties.Settings.Default.GamePath);
            }
            set => Properties.Settings.Default.GamePath = value?.FullName;
        }

        public static ClientLanguage GetLanguage()
        {
            return (ClientLanguage)Properties.Settings.Default.Language;
        }

        public static void SetLanguage(ClientLanguage language)
        {
            int previousLanguage = Properties.Settings.Default.Language;
            Properties.Settings.Default.Language = (int)language;

            if (previousLanguage != (int)language)
                LanguageChanged?.Invoke();
        }

        public static bool IsDX11()
        {
            return Properties.Settings.Default.IsDx11;
        }

        public static void SetDx11(bool value)
        {
            Properties.Settings.Default.IsDx11 = value;
        }

        public static bool IsAutologin()
        {
            return Properties.Settings.Default.AutoLogin;
        }

        public static void SetAutologin(bool value)
        {
            Properties.Settings.Default.AutoLogin = value;
        }

        public static bool NeedsOtp()
        {
            return Properties.Settings.Default.NeedsOtp;
        }

        public static void SetNeedsOtp(bool value)
        {
            Properties.Settings.Default.NeedsOtp = value;
        }

        public static bool SteamIntegrationEnabled
        {
            get => Properties.Settings.Default.SteamIntegrationEnabled;
            set => Properties.Settings.Default.SteamIntegrationEnabled = value;
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static string AdditionalLaunchArgs
        {
            get => Properties.Settings.Default.AdditionalLaunchArgs;
            set => Properties.Settings.Default.AdditionalLaunchArgs = value;
        }

        public static void StartOfficialLauncher(bool isSteam)
        {
            Process.Start(Path.Combine(GamePath.FullName, "boot", "ffxivboot.exe"), isSteam ? "-issteam" : string.Empty);
        }
    }
}
