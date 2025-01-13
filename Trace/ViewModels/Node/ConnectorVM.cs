using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Trace.ViewModels.Node
{
    public partial class ConnectorVM : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private Point _anchor;

        [ObservableProperty]
        private bool _isConnected;
    }
}
