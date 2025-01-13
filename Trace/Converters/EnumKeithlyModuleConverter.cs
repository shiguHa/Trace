
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Trace.Converters
{
    internal sealed class EnumKeithlyModuleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 実装する。
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is KeithleyModules module)
            //{
            //    return KeithleyModuleFactory.Create(module);
            //}
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
