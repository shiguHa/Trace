
using CommunityToolkit.Mvvm.ComponentModel;

namespace Trace.ViewModels.Node
{
    public abstract class EditorBaseVM : ObservableObject
    {
        public PendingConnectionVM PendingConnection { get; set; }

        public abstract void Connect(ConnectorVM source, ConnectorVM target);

    }
}
