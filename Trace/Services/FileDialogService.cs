
using Trace.Interfaces;

namespace Trace.Services
{
    internal class FileDialogService : IFileDialogService
    {

        public string OpenFileDialog(string filter)
        {
            return OpenFileDialog(filter, "");
        }

        public string OpenFileDialog(string filter, string initialDirectory)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = filter,
                RestoreDirectory = true,
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return "";
        }


    }
}
