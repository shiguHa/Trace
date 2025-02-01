
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Trace.ViewModels.Node
{
    public sealed partial class GroupingNodeVM : NodeBaseVM 
    {
        [ObservableProperty]
        private Size _actualSize;

        public GroupingNodeVM()
        {

        }
    }
}
