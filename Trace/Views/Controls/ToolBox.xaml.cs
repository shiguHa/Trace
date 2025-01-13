using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Trace.ControlInfos;
using Trace.ViewModels;

namespace Trace.Views.Controls
{
    /// <summary>
    /// ToolBox.xaml の相互作用ロジック
    /// </summary>
    public partial class ToolBox : UserControl
    {
        #region DataSource
        public ObservableCollection<Type> ToolBoxItems
        {
            get { return (ObservableCollection<Type>)GetValue(ToolBoxItemsProperty); }
            set { SetValue(ToolBoxItemsProperty, value); }
        }
        public static readonly DependencyProperty ToolBoxItemsProperty =
            DependencyProperty.Register(
                nameof(ToolBoxItems),
                typeof(ObservableCollection<Type>),
                typeof(ToolBox),
                new PropertyMetadata(new ObservableCollection<Type>(),
                    OnToolBoxItemsChanged));
        #endregion


        public ToolBox()
        {
            InitializeComponent();

            ToolBoxListBox.SetBinding(
                ListBox.ItemsSourceProperty,
                new Binding(nameof(ToolBoxItems))
                {
                    Source = this,
                    Mode = BindingMode.OneWay
                });

        }

        private static void OnToolBoxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //if (d is ToolBox toolbox)
            //{
            //    var newItems = e.NewValue as ObservableCollection<Type>;
            //    toolbox.ToolBoxListBox.ItemsSource = newItems;
            //}

        }

    }
}
