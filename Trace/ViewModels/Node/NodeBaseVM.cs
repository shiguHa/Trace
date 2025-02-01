
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Trace.ViewModels.Node
{
    public abstract partial class NodeBaseVM : ObservableObject
    {
        [ObservableProperty]
        private Point _location;

        [ObservableProperty]
        private Size _size;
    }
}
