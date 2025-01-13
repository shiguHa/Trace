

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Trace.Converters
{
    public class EnumToIntConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                return (int)(object)enumValue;
            }

            throw new ArgumentException("Value is not an Enum.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && targetType.IsEnum)
            {
                return Enum.ToObject(targetType, intValue);
            }

            throw new ArgumentException("Value is not an int or TargetType is not an Enum.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
