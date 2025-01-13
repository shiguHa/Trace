

using System.Collections.ObjectModel;

namespace Trace.ViewModels.Node
{
    public class NodeWithConnectorsVM : NodeBaseVM
    {

        public string Title { get; set; }   

        public ObservableCollection<ConnectorVM> Input { get; set; } = new ObservableCollection<ConnectorVM>();
        public ObservableCollection<ConnectorVM> Output { get; set; } = new ObservableCollection<ConnectorVM>();
    }
}
