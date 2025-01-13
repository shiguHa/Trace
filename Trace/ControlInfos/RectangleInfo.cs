using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Trace.ControlInfos
{
    public class RectangleInfo : ControlInfo
    {
        public Brush Fill { get; set; } = Brushes.Orange;

        public override FrameworkElement CreateControl()
        {
            return new Rectangle() { Fill = this.Fill };
        }
    }
}
