
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Trace.ControlInfos;
using Trace.Helpers;

namespace Trace.Views.Controls
{
    /// <summary>
    /// FormCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class FormCanvas : UserControl
    {

        private bool isDragging = false;
        private Point clickPosition;
        //private List<ControlInfo> selectedControls = new List<ControlInfo>();
        private IReadOnlyList<UIElement> selectedControls = new List<UIElement>();

        #region Controls DependencyProperty
        public ObservableCollection<ControlInfo> Controls
        {
            get { return (ObservableCollection<ControlInfo>)GetValue(ControlsProperty); }
            set { SetValue(ControlsProperty, value); }
        }

        public static readonly DependencyProperty ControlsProperty =
            DependencyProperty.Register(
                nameof(Controls),
                typeof(ObservableCollection<ControlInfo>),
                typeof(FormCanvas),
                new PropertyMetadata(new ObservableCollection<ControlInfo>(),
                    OnControlsChanged));

        #endregion

        public FormCanvas()
        {
            InitializeComponent();
        }

        private static void OnControlsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormCanvas formCanvas)
            {
                if (e.OldValue is ObservableCollection<ControlInfo> oldCollection)
                {
                    oldCollection.CollectionChanged -= formCanvas.Controls_CollectionChanged;
                }

                if (e.NewValue is ObservableCollection<ControlInfo> newCollection)
                {
                    newCollection.CollectionChanged += formCanvas.Controls_CollectionChanged;

                    foreach (var newItem in newCollection)
                    {
                        formCanvas.AddControlToCanvas(newItem as ControlInfo);
                    }
                }
            }
        }

        private void Controls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is null)
            {
                return;
            }

            foreach (var newItem in e.NewItems)
            {
                AddControlToCanvas(newItem as ControlInfo);
            }

        }

        private void AddControlToCanvas(ControlInfo controlInfo)
        {
            FrameworkElement control = controlInfo.CreateControl();

            if (control is null)
            {
                return;
            }

            control.IsHitTestVisible = false;

            ContentControl itemContent = new ContentControl

            {
                Template = (ControlTemplate)FindResource("DesignerItemTemplate"),
                Content = control,
            };

            Canvas.SetLeft(itemContent, controlInfo.X);
            Canvas.SetTop(itemContent, controlInfo.Y);
            MyCanvas.Children.Add(itemContent);


            itemContent.Tag = controlInfo;

        }


        private void multiSelectBehavior_SelectionChanged(object sender, Behavior.SelectionChangedEventArgs e)
        {
            selectedControls = e.SelectedItems;

            foreach (var element in e.RemovedItems)
            {
                if (element is ContentControl contentControl)
                {
                    SetMoveAndResizeVisibility(contentControl, Visibility.Hidden); //collapsedにするとRectangleの場合消える。
                }
            }

            foreach (var element in e.AddedItems)
            {
                if (element is ContentControl contentControl)
                {
                    SetMoveAndResizeVisibility(contentControl, Visibility.Visible);
                }
            }

        }

        private void SetMoveAndResizeVisibility(ContentControl contentControl, Visibility visibility)
        {
            var moveThumb = contentControl.FindVisualChild<Thumb>("MoveThumb");
            var resizeDecorator = contentControl.FindVisualChild<Control>("ResizeDecorator");

            if (moveThumb != null)
            {
                moveThumb.Visibility = visibility;
            }

            if (resizeDecorator != null)
            {
                resizeDecorator.Visibility = visibility;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (selectedControls is null && selectedControls.Count == 0)
            {
                return;
            }

            foreach (var control in selectedControls)
            {
                MoveControlWithinCanvas(control, e.HorizontalChange, e.VerticalChange);
            }

        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var resizeThumb = sender as Thumb;

            if (selectedControls is null && selectedControls.Count == 0)
            {
                return;
            }

            foreach (var control in selectedControls)
            {
                var item = control as Control;
                if (item is null)
                {
                    return;
                }

                double deltaVertical = 0.0, deltaHorizontal = 0.0;

                var isVertical = false;
                var isHorizontal = false;

                if (resizeThumb.VerticalAlignment == VerticalAlignment.Top)
                {
                    deltaVertical = Math.Min(e.VerticalChange, item.ActualHeight - item.MinHeight);
                    Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                    isVertical = true;
                }

                if (resizeThumb.VerticalAlignment == VerticalAlignment.Bottom)
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

                if (resizeThumb.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    deltaHorizontal = Math.Min(e.HorizontalChange, item.ActualWidth - item.MinWidth);
                    Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                    isHorizontal = true;
                }

                if (resizeThumb.HorizontalAlignment == HorizontalAlignment.Right)
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
            }


        }


        private void MoveControlWithinCanvas(UIElement control, double horizontalChange, double verticalChange)
        {
            double left = Canvas.GetLeft(control);
            double top = Canvas.GetTop(control);

            double newLeft = left + horizontalChange;
            double newTop = top + verticalChange;

            // Canvasの境界を超えないように制限
            if (newLeft < 0) newLeft = 0;
            if (newTop < 0) newTop = 0;
            if (newLeft + control.RenderSize.Width > MyCanvas.ActualWidth) newLeft = MyCanvas.ActualWidth - control.RenderSize.Width;
            if (newTop + control.RenderSize.Height > MyCanvas.ActualHeight) newTop = MyCanvas.ActualHeight - control.RenderSize.Height;

            Canvas.SetLeft(control, newLeft);
            Canvas.SetTop(control, newTop);
        }


    }



}
