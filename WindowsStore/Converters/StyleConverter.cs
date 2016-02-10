﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WindowsStore.Converters
{
    public class StyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? (Style)App.Current.Resources["PauseButtonStyle"] :
                (Style)App.Current.Resources["PlayButtonStyle"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
