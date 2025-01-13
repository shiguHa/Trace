
using System.Windows;
using System.Windows.Controls;

namespace Trace.ControlInfos
{
    public abstract class ControlInfo
    {
        public double X { get; set; }
        public double Y { get; set; }

        public abstract FrameworkElement CreateControl();
    }
}
