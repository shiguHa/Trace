using System.Windows.Controls;
using Trace.ViewModels;

namespace Trace.Views
{
    /// <summary>
    /// HomePage.xaml の相互作用ロジック
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage(HomeVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
