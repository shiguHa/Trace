using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace Trace.Behavior
{
    public class MultiSelectBehavior : Behavior<Canvas>
    {
        private bool isDragging;
        private Point startPoint;
        private Rectangle selectionRectangle;

        #region 依存プロパティ SelectedElements

        public static readonly DependencyProperty SelectedElementsProperty =
            DependencyProperty.Register(
                nameof(SelectedElements),
                typeof(ObservableCollection<UIElement>),
                typeof(MultiSelectBehavior),
                new PropertyMetadata(null, OnSelectedElementsChanged));

        public ObservableCollection<UIElement> SelectedElements
        {
            get => (ObservableCollection<UIElement>)GetValue(SelectedElementsProperty);
            set => SetValue(SelectedElementsProperty, value);
        }

        #endregion

        #region 依存プロパティ SelectionChangedCommand

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(MultiSelectBehavior),
                new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }
        #endregion

        #region 依存プロパティ SelectionRectangleBackground
        public Brush SelectionRectangleBackground
        {
            get { return (Brush)GetValue(SelectionRectangleBackgroundProperty); }
            set { SetValue(SelectionRectangleBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectionRectangleBackgroundProperty = DependencyProperty.Register(
            nameof(SelectionRectangleBackgroundProperty),
            typeof(Brush),
            typeof(MultiSelectBehavior),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50, 173, 216, 240))));
        #endregion

        #region 依存プロパティ SelectionRectangleBackground
        public Brush SelectionRectangleStroke
        {
            get { return (Brush)GetValue(SelectionRectangleStrokeProperty); }
            set { SetValue(SelectionRectangleStrokeProperty, value); }
        }

        public static readonly DependencyProperty SelectionRectangleStrokeProperty = DependencyProperty.Register(
            nameof(SelectionRectangleStrokeProperty),
            typeof(Brush),
            typeof(MultiSelectBehavior),
            new PropertyMetadata(new SolidColorBrush(Colors.DarkBlue)));
        #endregion

        #region 依存プロパティ SelectionRectangleBackground
        public double SelectionRectangleStrokeThickness
        {
            get { return (double)GetValue(SelectionRectangleStrokeThicknessProperty); }
            set { SetValue(SelectionRectangleStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty SelectionRectangleStrokeThicknessProperty = DependencyProperty.Register(
            nameof(SelectionRectangleStrokeThicknessProperty),
            typeof(double),
            typeof(MultiSelectBehavior),
            new PropertyMetadata(1.0));
        #endregion

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        protected override void OnAttached()
        {
            base.OnAttached();
            SelectedElements = new ObservableCollection<UIElement>();
            SelectedElements.CollectionChanged += OnSelectedElementsCollectionChanged;
            selectionRectangle = new Rectangle
            {
                Stroke = SelectionRectangleStroke,
                StrokeThickness = SelectionRectangleStrokeThickness,
                Fill = SelectionRectangleBackground,
                Visibility = Visibility.Collapsed
            };
            AssociatedObject.Children.Add(selectionRectangle);

            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            SelectedElements.CollectionChanged -= OnSelectedElementsCollectionChanged;
        }

        private void OnSelectedElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var addedItems = new List<UIElement>();
            var removedItems = new List<UIElement>();

            if (e.NewItems != null)
            {
                foreach (UIElement item in e.NewItems)
                {
                    addedItems.Add(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (UIElement item in e.OldItems)
                {
                    removedItems.Add(item);
                }
            }

            var args = new SelectionChangedEventArgs(SelectedElements, addedItems, removedItems);
            SelectionChanged?.Invoke(this, args);
            SelectionChangedCommand?.Execute(args);
        }

        private static void OnSelectedElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiSelectBehavior behavior)
            {
                if (e.OldValue is ObservableCollection<UIElement> oldCollection)
                {
                    oldCollection.CollectionChanged -= behavior.OnSelectedElementsCollectionChanged;
                }

                if (e.NewValue is ObservableCollection<UIElement> newCollection)
                {
                    newCollection.CollectionChanged += behavior.OnSelectedElementsCollectionChanged;
                }
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as UIElement;

            // e.OriginalSourceがCanvasの場合、クリック位置の要素を取得
            if (element == AssociatedObject)
            {
                Point clickPosition = e.GetPosition(AssociatedObject);
                HitTestResult hitTestResult = VisualTreeHelper.HitTest(AssociatedObject, clickPosition);
                if (hitTestResult != null)
                {
                    element = hitTestResult.VisualHit as UIElement;
                }
            }

            // Canvasの直下の子要素を取得する。
            while (element != null && element != AssociatedObject)
            {
                if (AssociatedObject.Children.Contains(element))
                {
                    break;
                }
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (element != null && element != AssociatedObject)
                {
                    if (SelectedElements.Contains(element))
                    {
                        SelectedElements.Remove(element);
                    }
                    else
                    {
                        SelectedElements.Add(element);
                    }
                }
            }
            else
            {
                if (element != AssociatedObject)
                {
                    // Canvas以外追加されていない場合
                    if (SelectedElements.Contains(element) == false)
                    {
                        SelectedElements.Add(element);
                    }
                    return;
                }

                isDragging = true;
                startPoint = e.GetPosition(AssociatedObject);
                selectionRectangle.Width = 0;
                selectionRectangle.Height = 0;
                Canvas.SetLeft(selectionRectangle, startPoint.X);
                Canvas.SetTop(selectionRectangle, startPoint.Y);
                selectionRectangle.Visibility = Visibility.Visible;
                AssociatedObject.CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging == false)
            {
                return;
            }

            var pos = e.GetPosition(AssociatedObject);
            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);
            var width = Math.Abs(pos.X - startPoint.X);
            var height = Math.Abs(pos.Y - startPoint.Y);

            selectionRectangle.Width = width;
            selectionRectangle.Height = height;
            Canvas.SetLeft(selectionRectangle, x);
            Canvas.SetTop(selectionRectangle, y);

        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging == false)
            {
                return;
            }

            isDragging = false;
            selectionRectangle.Visibility = Visibility.Collapsed;
            AssociatedObject.ReleaseMouseCapture();
            UpdateSelection();

        }

        private void UpdateSelection()
        {
            var selectionBounds = new Rect(Canvas.GetLeft(selectionRectangle), Canvas.GetTop(selectionRectangle), selectionRectangle.Width, selectionRectangle.Height);
            foreach (UIElement element in AssociatedObject.Children)
            {
                if (element is Rectangle) continue;

                var elementBounds = new Rect(Canvas.GetLeft(element), Canvas.GetTop(element), element.RenderSize.Width, element.RenderSize.Height);
                if (selectionBounds.IntersectsWith(elementBounds))
                {
                    if (!SelectedElements.Contains(element))
                    {
                        SelectedElements.Add(element);
                        element.SetValue(Panel.BackgroundProperty, Brushes.LightBlue);
                    }
                }
                else
                {
                    if (SelectedElements.Contains(element))
                    {
                        SelectedElements.Remove(element);
                        element.ClearValue(Panel.BackgroundProperty);
                    }
                }
            }
        }
    }

    public class SelectionChangedEventArgs : EventArgs
    {
        public IReadOnlyList<UIElement> SelectedItems { get; }
        public IReadOnlyList<UIElement> AddedItems { get; }
        public IReadOnlyList<UIElement> RemovedItems { get; }

        public SelectionChangedEventArgs(IReadOnlyList<UIElement> selectedElements, IReadOnlyList<UIElement> addedItems, IReadOnlyList<UIElement> removedItems)
        {
            SelectedItems = selectedElements;
            AddedItems = addedItems;
            RemovedItems = removedItems;
        }
    }
}
