

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Trace.ViewModels.Node
{
    public sealed partial class NodeDBTableVM : NodeBaseVM
    {
        [ObservableProperty]
        private string _tableName;

        public ObservableCollection<ColumnVM> Columns { get; set; }

        private readonly ObservableCollection<ConnectorVM> _inputCollection = new();
        public ReadOnlyObservableCollection<ConnectorVM> Input { get; }

        private readonly ObservableCollection<ConnectorVM> _outputCollection = new();
        public ReadOnlyObservableCollection<ConnectorVM> Output { get; }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NodeDBTableVM()
        {
            Columns = new ObservableCollection<ColumnVM>();
            Input = new ReadOnlyObservableCollection<ConnectorVM>(_inputCollection);
            Output = new ReadOnlyObservableCollection<ConnectorVM>(_outputCollection);
        }

        // カラムを追加するメソッド
        public void AddColumn(string columnName, string dataType)
        {
            Columns.Add(new ColumnVM { ColumnName = columnName, DataType = dataType });
        }

        public void SetInput(ConnectorVM input)
        {
            _inputCollection.Clear();
            _inputCollection.Add(input);
        }

        public void SetOutput(ConnectorVM output)
        {
            _outputCollection.Clear();
            _outputCollection.Add(output);
        }
    }

    // カラムを表現するクラス
    public class ColumnVM
    {
        public string ColumnName { get; set; } = "default";
        public string DataType { get; set; } = "string";

    }
}
