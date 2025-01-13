

using System.Windows;
using System.Windows.Controls;

namespace Trace.ControlInfos
{
    public sealed class TextBlockInfo : ControlInfo
    {
        public string Text { get; set; }

        public override FrameworkElement CreateControl()
        {
            return new TextBlock() { Text = Text };
        }
    }

}
