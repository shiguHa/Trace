using CommunityToolkit.Mvvm.Input;


namespace Trace.ViewModels.Node
{
    public partial class PendingConnectionVM
    {
        private readonly EditorBaseVM _editor;

        private ConnectorVM _source;

        public PendingConnectionVM(EditorBaseVM editor)
        {
            _editor = editor;
        }

        [RelayCommand]
        public void Start(ConnectorVM source)
        {
            _source = source;
        }


        [RelayCommand]
        public void Finish(ConnectorVM target)
        {
            if (target != null)
                _editor.Connect(_source, target);
        }

    }
}
