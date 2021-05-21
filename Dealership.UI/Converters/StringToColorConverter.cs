using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Dealership.UI.Converters
{
    public class StringToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if(new BrushConverter().IsValid($"#{str}"))
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{str}"));
                    else
                    {
                        return new SolidColorBrush(Colors.Black);
                    }
                }

                return default;
            }

            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
