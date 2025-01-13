

namespace Trace.Interfaces
{
    public interface IFileDialogService
    {
        string OpenFileDialog(string filter);
        string OpenFileDialog(string filter, string initialDirectory);

    }
}
