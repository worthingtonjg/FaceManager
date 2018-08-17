using System;
using Windows.UI.Xaml.Controls;

namespace FaceApiManager.Common
{
    public static class NavigationHelper
    {
        public static Frame NavigationFrame { get; set; }

        public static bool Navigate(Type page, object parameter = null)
        {
            return NavigationFrame.Navigate(page, parameter);
        }

    }
}
