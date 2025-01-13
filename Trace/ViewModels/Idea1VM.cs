using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nodify;
using System.Collections.ObjectModel;
using Trace.ViewModels.Node;

namespace Trace.ViewModels
{
    public partial class Idea1VM : ObservableObject
    {
        public Idea1NodeEditorVM NodeEditor { get; } = new();


        [RelayCommand]
        private void NewNode()
        {
            var newNode = new NodeWithConnectorsVM
            {
                Title = "Welcome",
                Input = new ObservableCollection<ConnectorVM>
            {
                new ConnectorVM
                {
                    Title = "In"
                }
            },
                Output = new ObservableCollection<ConnectorVM>
            {
                new ConnectorVM
                {
                    Title = "Out"
                }
            }
            };

            NodeEditor.Nodes.Add(newNode);
        }
    }

    public partial class Idea1NodeEditorVM : EditorBaseVM
    {
        public ObservableCollection<NodeBaseVM> Nodes { get; } = new();
        public ObservableCollection<ConnectionVM> Connections { get; } = new();

        public PendingConnectionVM PendingConnection { get; }

        public Idea1NodeEditorVM()
        {
            this.PendingConnection = new(this);
            var welcome = new NodeWithConnectorsVM
            {
                Title = "Welcome",
                Input = new ObservableCollection<ConnectorVM>
            {
                new ConnectorVM
                {
                    Title = "In"
                }
            },
                Output = new ObservableCollection<ConnectorVM>
            {
                new ConnectorVM
                {
                    Title = "Out"
                }
            }
            };

            var nodify = new NodeWithConnectorsVM
            {
                Title = "To Nodify",
                Input = new ObservableCollection<ConnectorVM>
            {
                new ConnectorVM
                {
                    Title = "In"
                }
            }
            };

            Nodes.Add(welcome);
            Nodes.Add(nodify);

            this.Connect(welcome.Output[0], nodify.Input[0]);
        }

        public override void Connect(ConnectorVM source, ConnectorVM target)
        {
            Connections.Add(new ConnectionVM(source, target));
        }

    }
}
