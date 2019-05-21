using System;
using System.Globalization;
using Xamarin.Forms;

namespace IndoorPositioning
{
    public class RssiToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Color.WhiteSmoke;
            var rssi = System.Convert.ToInt32(value);
            if (rssi > -65)
            {
                return Color.Green;
            }

            if (rssi <= -65 && rssi >= -85)
            {
                return Color.Gold;
            }

            if (rssi < -85)
            {
                return Color.Red;
            }

            return Color.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}