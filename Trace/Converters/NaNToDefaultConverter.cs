using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Trace.Converters
{
        public class NaNToDefaultConverter : MarkupExtension, IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is double doubleValue && double.IsNaN(doubleValue))
                {
                    return parameter != null ? System.Convert.ToDouble(parameter) : 0.0;
                }
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

