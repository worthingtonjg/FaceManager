using Windows.Storage;

namespace FaceApiManager.Common
{
    public class SettingsHelper
    {
        private static string _faceApiSubscriptionKey = "FaceApiSubscriptionKey";
        private static string _faceApiRoot = "FaceApiRoot";

        public static string FaceApiSubscriptionKey
        {
            get
            {
                return ReadSettings(_faceApiSubscriptionKey);
            }

            set
            {
                SaveSetting(_faceApiSubscriptionKey, value);
            }
        }

        public static string FaceApiRoot
        {
            get
            {
                return ReadSettings(_faceApiRoot);
            }

            set
            {
                SaveSetting(_faceApiRoot, value);
            }
        }


        private static void SaveSetting(string name, string value)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values[name] = value;
        }

        private static string ReadSettings(string name)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(name))
            {
                return localSettings.Values[name].ToString();
            }

            return string.Empty;
        }
    }
}
