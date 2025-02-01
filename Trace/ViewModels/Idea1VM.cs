using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nodify;
using System.Collections.ObjectModel;
using System.Windows;
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

            var n = NodeEditor.GetNodesWithinGroupingNode(NodeEditor.Nodes[2] as GroupingNodeVM);
        }
    }

    public partial class Idea1NodeEditorVM : EditorBaseVM
    {
        public ObservableCollection<NodeBaseVM> Nodes { get; } = new();

        public ObservableCollection<NodeBaseVM> SelectedNodes { get; } = new();

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
                },
                In = new ConnectorVM { Title = "In" },
                Out = new ConnectorVM { Title = "Out" }
            };

            var testTable2 = new NodeDBTableVM
            {
                TableName = "TestTable2",

                Columns = new ObservableCollection<ColumnVM>
                {
                    new ColumnVM { ColumnName = "Column1", DataType = "int" },
                    new ColumnVM { ColumnName = "Column2", DataType = "string" }
                },
                In = new ConnectorVM { Title = "In" },
                Out = new ConnectorVM { Title = "Out" }
            };


            var group = new GroupingNodeVM();

            Nodes.Add(testTable);
            Nodes.Add(testTable2);
            Nodes.Add(group);



        }

        public override void Connect(ConnectorVM source, ConnectorVM target)
        {
            Connections.Add(new ConnectionVM(source, target));
        }

        public IEnumerable<NodeBaseVM> GetNodesWithinGroupingNode(GroupingNodeVM groupingNode)
        {
            var nodesWithin = new List<NodeBaseVM>();

            foreach (var node in Nodes)
            {
                if (node != groupingNode && IsNodeWithinGroupingNode(node, groupingNode) && SelectedNodes.Contains(node))
                {
                    nodesWithin.Add(node);
                }
            }

            return nodesWithin;
        }

        private bool IsNodeWithinGroupingNode(NodeBaseVM node, GroupingNodeVM groupingNode)
        {
            var nodeRect = new Rect(node.Location, node.Size);
            var groupingNodeRect = new Rect(groupingNode.Location, groupingNode.ActualSize);

            return groupingNodeRect.Contains(nodeRect);
        }

    }
}
