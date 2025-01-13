

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Trace.ViewModels.Node
{
    public sealed partial class NodeDBTableVM : NodeBaseVM
    {
        [ObservableProperty]
        private string _tableName;

        public ObservableCollection<ColumnVM> Columns;

        public NodeDBTableVM()
        {
            Columns = new ObservableCollection<ColumnVM>();
        }

        // カラムを追加するメソッド
        public void AddColumn(string columnName, string dataType)
        {
            Columns.Add(new ColumnVM { ColumnName = columnName, DataType = dataType });
        }
    }

    // カラムを表現するクラス
    public class ColumnVM
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
