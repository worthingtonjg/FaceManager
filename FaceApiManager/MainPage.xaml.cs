using FaceApiManager.Common;
using FaceApiManager.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FaceApiManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavigationHelper.NavigationFrame = NavigationFrame;

            bool needSettings = false;
            if (SettingsHelper.FaceApiSubscriptionKey == string.Empty) needSettings = true;
            if (SettingsHelper.FaceApiRoot == string.Empty) needSettings = true;

            if (needSettings)
            {
                NavigationHelper.Navigate(typeof(SettingsPage));
            }
            else
            {
                NavigationHelper.Navigate(typeof(ManagePersonGroupsPage));
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationHelper.Navigate(typeof(SettingsPage));
            }
        }
    }
}
