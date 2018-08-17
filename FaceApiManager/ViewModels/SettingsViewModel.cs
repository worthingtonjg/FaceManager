using FaceApiManager.Common;
using FaceApiManager.Pages;
using System.Windows.Input;

namespace FaceApiManager.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            SetValue(() => FaceApiSubscriptionKey, SettingsHelper.FaceApiSubscriptionKey);
            SetValue(() => FaceApiRoot, SettingsHelper.FaceApiRoot);
        }

        [XamlProperty]
        public string FaceApiSubscriptionKey { get; set; }

        [XamlProperty]
        public string FaceApiRoot { get; set; }

        [XamlProperty]
        public ICommand BackCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    NavigationHelper.Navigate(typeof(ManagePersonGroupsPage));
                });
            }
        }

        [XamlProperty]
        public ICommand SaveSettingsCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    SettingsHelper.FaceApiSubscriptionKey = FaceApiSubscriptionKey;
                    SettingsHelper.FaceApiRoot = FaceApiRoot;


                    NavigationHelper.Navigate(typeof(ManagePersonGroupsPage));
                });
            }
        }
    }
}
