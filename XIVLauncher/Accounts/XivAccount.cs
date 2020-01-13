using System.Net;
using AdysTech.CredentialManager;
using Newtonsoft.Json;

namespace XIVLauncher.Accounts
{
    public class XivAccount
    {
        [JsonIgnore]
        public string Id => $"{UserName}-{UseOtp}-{UseSteamServiceAccount}";

        public string UserName { get; private set; }

        [JsonIgnore]
        public string Password
        {
            get
            {
                var credentials = CredentialManager.GetCredentials($"FINAL FANTASY XIV-{UserName}");

                return credentials != null ? credentials.Password : string.Empty;
            }
            set => CredentialManager.SaveCredentials($"FINAL FANTASY XIV-{UserName}", new NetworkCredential
                {
                    UserName = UserName,
                    Password = value
                });
        }

        public bool SavePassword { get; set; }
        public bool UseSteamServiceAccount { get; set; }
        public bool UseOtp { get; set; }

        public string ChosenCharacterName;
        public string ChosenCharacterWorld;

        public string ThumbnailUrl;

        public XivAccount(string userName)
        {
            UserName = userName;
        }

        public string FindCharacterThumb()
        {
            return null;
        }
    }
}
