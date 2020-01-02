
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using XIVLauncher.Game;

namespace XIVLauncher.Settings
{
    public class LauncherSettings
    {
        #region Launcher Setting
        public DirectoryInfo GamePath { get; set; }
        public bool IsDx11 { get; set; }
        public bool AutologinEnabled { get; set; }
        public bool NeedsOtp { get; set; }

        public bool CharacterSyncEnabled { get; set; }
        public string AdditionalLaunchArgs { get; set; }
        public bool SteamIntegrationEnabled { get; set; }
        public ClientLanguage Language { get; set; }
        public string CurrentAccountId { get; set; }

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

        public static void SetNeedsOtp(bool value)
        {
            Properties.Settings.Default.NeedsOtp = value;
        }
        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher", "launcherConfig.json");

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

        public static LauncherSettings Load()
        {
            if (!File.Exists(ConfigPath))
                return new LauncherSettings();

            var setting = JsonConvert.DeserializeObject<LauncherSettings>(File.ReadAllText(ConfigPath), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            setting.AddonList = EnsureDefaultAddon(setting.AddonList);

            return setting;
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
