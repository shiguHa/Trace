

namespace Trace.ViewModels.Node
{
    public  class ConnectionVM
    {
        public ConnectionVM(ConnectorVM source, ConnectorVM target)
        {
            Source = source;
            Target = target;

            Source.IsConnected = true;
            Target.IsConnected = true;
        }

        public ConnectorVM Source { get; }
        public ConnectorVM Target { get; }
    }
}
