
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Trace.Views.Controls
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control item = this.DataContext as Control;

            if (item is null)
            {
                return;
            }

            double deltaVertical = 0.0, deltaHorizontal = 0.0;

            var isVertical = false;
            var isHorizontal = false;

            if (this.VerticalAlignment == VerticalAlignment.Top)
            {
                deltaVertical = Math.Min(e.VerticalChange, item.ActualHeight - item.MinHeight);
                Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                isVertical = true;
            }

            if (this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                deltaVertical = Math.Min(-e.VerticalChange, item.ActualHeight - item.MinHeight);
                isVertical = true;
            }

            if (isVertical)
            {
                if (item.Height is double.NaN)
                {
                    item.Height = item.ActualHeight - deltaVertical;
                }
                else
                {
                    item.Height -= deltaVertical;
                }
            }

            if (this.HorizontalAlignment == HorizontalAlignment.Left)
            {
                deltaHorizontal = Math.Min(e.HorizontalChange, item.ActualWidth - item.MinWidth);
                Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                isHorizontal = true;
            }

            if (this.HorizontalAlignment == HorizontalAlignment.Right)
            {
                deltaHorizontal = Math.Min(-e.HorizontalChange, item.ActualWidth - item.MinWidth);
                isHorizontal = true;
            }

            if (isHorizontal)
            {
                if (item.Width is double.NaN)
                {
                    item.Width = item.ActualHeight - deltaHorizontal;
                }
                else
                {
                    item.Width -= deltaHorizontal;
                }
            }


            e.Handled = true;
        }
    }
}
