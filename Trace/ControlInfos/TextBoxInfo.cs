

using System.Windows;
using System.Windows.Controls;

namespace Trace.ControlInfos
{
    public sealed class TextBoxInfo : ControlInfo
    {
        public string Text { get; set; }

        public override FrameworkElement CreateControl()
        {
            return new TextBox() { Text = Text };
        }
    }
}
