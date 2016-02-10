using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WindowsStore.UIExtensions
{
    public static class SettingsCharmExtensions
    {
        public static Popup BuildSettingsItem(UserControl userControl, int width)
        {
            var popup = new Popup();

            popup.IsLightDismissEnabled = true;
            userControl.Width = width;
            userControl.Height = Window.Current.Bounds.Height;

            popup.Child = userControl;

            popup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - width);
            popup.SetValue(Canvas.TopProperty, 0);
            popup.IsOpen = true;

            return popup;
        }
    }
}
