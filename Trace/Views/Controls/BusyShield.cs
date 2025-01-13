//https://shuntaro3.hatenablog.com/entry/2018/10/01/002603

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Trace.Views.Controls
{
    public class BusyShield : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(BusyShield), new PropertyMetadata(false, OnIsBusyChanged));
        #endregion

        #region BusyMessage
        public string BusyMessage
        {
            get { return (string)GetValue(BusyMessageProperty); }
            set { SetValue(BusyMessageProperty, value); }
        }

        public static readonly DependencyProperty BusyMessageProperty = DependencyProperty.Register(
            nameof(BusyMessageProperty), typeof(string), typeof(BusyShield), new PropertyMetadata("お待ちください"));
        #endregion

        #region BusyBackground
        public Brush BusyBackground
        {
            get { return (Brush)GetValue(BusyBackgroundProperty); }
            set { SetValue(BusyBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BusyBackgroundProperty = DependencyProperty.Register(
            nameof(BusyBackgroundProperty), typeof(Brush), typeof(BusyShield), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        #endregion

        #region IndicatorColor
        public Brush IndicatorColor
        {
            get { return (Brush)GetValue(IndicatorColorProperty); }
            set { SetValue(IndicatorColorProperty, value); }
        }

        public static readonly DependencyProperty IndicatorColorProperty = DependencyProperty.Register(
            nameof(IndicatorColorProperty), typeof(Brush), typeof(BusyShield), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        #endregion

        #region IndicatorWidth
        public double IndicatorWidth
        {
            get { return (double)GetValue(IndicatorWidthProperty); }
            set { SetValue(IndicatorWidthProperty, value); }
        }

        public static readonly DependencyProperty IndicatorWidthProperty = DependencyProperty.Register(
            nameof(IndicatorWidthProperty), typeof(double), typeof(BusyShield), new PropertyMetadata(50.0));
        #endregion

        #region IndicatorHeight
        public double IndicatorHeight
        {
            get { return (double)GetValue(IndicatorHeightProperty); }
            set { SetValue(IndicatorHeightProperty, value); }
        }

        public static readonly DependencyProperty IndicatorHeightProperty = DependencyProperty.Register(
            nameof(IndicatorHeightProperty), typeof(double), typeof(BusyShield), new PropertyMetadata(50.0));
        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Create Grid
            var grid = new Grid();

            // Create ContentPresenter and add to Grid
            var contentPresenter = new ContentPresenter()
            {
                Content = Content

            };

            grid.Children.Add(contentPresenter);

            #region busyの時に表示される
            // Create Rectangle and add to Grid
            var rectangle = new Rectangle
            {
                Fill = BusyBackground,
                Opacity = 0.7
            };
            grid.Children.Add(rectangle);


            var stackPanel = new StackPanel()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            grid.Children.Add(stackPanel);

            // Create ProgressBar and add to StackPanel
            var progressBar = new ProgressBar
            {
                IsIndeterminate = true,
                Width = IndicatorWidth,
                Height = IndicatorHeight,
                Style = FindResource("MaterialDesignCircularProgressBar") as Style,
            };
            stackPanel.Children.Add(progressBar);

            // Create TextBlock and add to StackPanel
            var textBlock = new TextBlock
            {
                Text = BusyMessage,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 15,
            };
            stackPanel.Children.Add(textBlock);




            #endregion

            // Bind StackPanel and Rectangle visibility to IsBusy
            var visibilityBinding = new Binding(nameof(IsBusy))
            {
                Source = this,
                Converter = new BooleanToVisibilityConverter()
            };
            stackPanel.SetBinding(VisibilityProperty, visibilityBinding);
            rectangle.SetBinding(VisibilityProperty, visibilityBinding);

            // Set UserControl's content to the Grid
            Content = grid;
        }



        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BusyShield)d;
            control.OnPropertyChanged(nameof(IsBusy));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
