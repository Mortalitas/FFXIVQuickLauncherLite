using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdysTech.CredentialManager;
using Newtonsoft.Json;
using XIVLauncher.Game;

namespace XIVLauncher
{
    // TODO: All of this needs a rework
    static class Settings
    {
        public static Action LanguageChanged;

        public static NetworkCredential GetCredentials(string app)
        {
            return CredentialManager.GetCredentials(app);
        }

        public static void SaveCredentials(string app, string username, string password)
        {
            CredentialManager.SaveCredentials(app, new NetworkCredential(username, password));
        }

        public static void ResetCredentials(string app)
        {
            if (CredentialManager.GetCredentials(app) != null)
            {
                CredentialManager.RemoveCredentials(app);
            }
        }

        public static string GetGamePath()
        {
            return Properties.Settings.Default.GamePath;
        }

        public static void SetGamePath(string path)
        {
            Properties.Settings.Default.GamePath = path;
        }

        public static ClientLanguage GetLanguage()
        {
            return (ClientLanguage) Properties.Settings.Default.Language;
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

    }
}
