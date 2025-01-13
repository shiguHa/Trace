
using System.Windows.Data;
using System.Windows.Markup;

namespace Trace.Converters
{
    public sealed class StringToBoolConverter : MarkupExtension, IValueConverter
    {
        public string FalseValue { get; set; }
        public string? FalseValue2 { get; set; }

        public string TrueValue { get; set; }

        public string? TrueValue2 { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                if (str == TrueValue) return true;
                if (str == TrueValue2) return true;

                if (str == FalseValue) return false;
                if (str == FalseValue2) return false;

            }

            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


}
