using BilibiliVideoFetcher.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BilibiliVideoFetcher.Views
{
    [ValueConversion(typeof(NotificationLevel), typeof(ImageSource))]
    public class NotificationLevelToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lv = (NotificationLevel)value;

            switch (lv)
            {
                case NotificationLevel.Debug:
                    return this.DebugImage;
                case NotificationLevel.Info:
                    return this.InfoImage;
                case NotificationLevel.Warning:
                    return this.WarningImage;
                case NotificationLevel.Error:
                    return this.ErrorImage;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        public ImageSource InfoImage { get; set; }

        public ImageSource DebugImage { get; set; }

        public ImageSource ErrorImage { get; set; }

        public ImageSource WarningImage { get; set; }
    }
}
