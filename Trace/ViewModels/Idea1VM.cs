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



            var testTable = new NodeDBTableVM
            {
                TableName = "TestTable",
                Columns = new ObservableCollection<ColumnVM>
                {
                    new ColumnVM { ColumnName = "Column1", DataType = "int" },
                    new ColumnVM { ColumnName = "Column2", DataType = "string" }
                }
            };

            testTable.SetInput(new ConnectorVM { Title = "In" });
            testTable.SetOutput(new ConnectorVM { Title = "Out" });
            Nodes.Add(testTable);

        }

        public override void Connect(ConnectorVM source, ConnectorVM target)
        {
            Connections.Add(new ConnectionVM(source, target));
        }

    }
}
