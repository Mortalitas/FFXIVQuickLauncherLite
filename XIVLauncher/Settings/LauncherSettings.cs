using Newtonsoft.Json;
using Serilog;
using System;
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

        #endregion

        #region SaveLoad

        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncherLite", "launcherConfig.json");

        public void Save()
        {
            Log.Information("Saving LauncherSettings to {0}", ConfigPath);

            File.WriteAllText(ConfigPath,  JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            }));
        }

        public static LauncherSettings Load()
        {
            if (!File.Exists(ConfigPath))
            {
                Log.Information("LauncherSettings at {0} does not exist, creating new...", ConfigPath);
                return new LauncherSettings();
            }

            var setting = JsonConvert.DeserializeObject<LauncherSettings>(File.ReadAllText(ConfigPath), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            Log.Information("Loaded LauncherSettings at {0}", ConfigPath);

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
