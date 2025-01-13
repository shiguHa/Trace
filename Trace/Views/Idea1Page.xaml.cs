using System.Windows.Controls;
using Trace.ViewModels;

namespace Trace.Views
{
    /// <summary>
    /// Idea1Page.xaml の相互作用ロジック
    /// </summary>
    public partial class Idea1Page : Page
    {
        public Idea1Page(Idea1VM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
