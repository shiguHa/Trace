using System.Windows.Markup;

namespace Trace.Helpers
{
    public sealed class EnumBindingExtension : MarkupExtension
    {
        public Type EnumType;

        public EnumBindingExtension(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"{enumType} is not enum.");
            }

            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null)
                throw new InvalidOperationException("The EnumType must be specified.");

            return Enum.GetValues(EnumType);
        }
    }
}
