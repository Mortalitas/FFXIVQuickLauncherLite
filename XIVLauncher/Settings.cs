
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using XIVLauncher.Game;

namespace XIVLauncher
{
    class Settings
    {
        public Action LanguageChanged;

        #region Launcher Setting
        public DirectoryInfo GamePath { get; set; }
        public bool IsDx11 { get; set; }
        public bool AutologinEnabled { get; set; }
        public bool NeedsOtp { get; set; }

        public bool CharacterSyncEnabled { get; set; }
        public string AdditionalLaunchArgs { get; set; }
        public bool SteamIntegrationEnabled { get; set; }

        
        private ClientLanguage _internalLang;
        [JsonIgnore]
        public ClientLanguage Language
        {
            return Properties.Settings.Default.IsDx11;
        }
            get => _internalLang;
            set
            {
                if (_internalLang != value)
                    LanguageChanged?.Invoke();

        public static void SetDx11(bool value)
        {
            Properties.Settings.Default.IsDx11 = value;
                _internalLang = value;
            }
        }

        public static bool IsAutologin()
        {
            return Properties.Settings.Default.AutoLogin;
        }
        #endregion

        public static void SetAutologin(bool value)
        {
            Properties.Settings.Default.AutoLogin = value;
        }
        #region SaveLoad

        public static bool NeedsOtp()
        {
            return Properties.Settings.Default.NeedsOtp;
        }

        public static void SetNeedsOtp(bool value)
        {
            Properties.Settings.Default.NeedsOtp = value;
        }
        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher", "launcherConfig.json");

        public static bool SteamIntegrationEnabled
        public void Save()
        {
            get => Properties.Settings.Default.SteamIntegrationEnabled;
            set => Properties.Settings.Default.SteamIntegrationEnabled = value;
            File.WriteAllText(ConfigPath,  JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            }));
        }

        public static Settings Load()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(ConfigPath), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        #endregion

        #region Misc

        public void StartOfficialLauncher(bool isSteam)
        {
            Process.Start(Path.Combine(GamePath.FullName, "boot", "ffxivboot.exe"), isSteam ? "-issteam" : string.Empty);
        }

        #endregion
    }
}
