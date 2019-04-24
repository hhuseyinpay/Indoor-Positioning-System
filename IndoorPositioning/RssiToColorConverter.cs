using System;
using System.Globalization;
using Xamarin.Forms;

namespace IndoorPositioning
{
    public class RssiToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return Color.WhiteSmoke;
            var rssi = System.Convert.ToInt32(value);
            if (rssi > -75)
            {
                return Color.Green;
            }

            if (rssi <= -75 && rssi >= -90)
            {
                return Color.Gold;
            }

            if (rssi < -90)
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