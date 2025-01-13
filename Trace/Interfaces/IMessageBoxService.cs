
using System.Windows;

namespace Trace.Interfaces
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message);

        MessageBoxResult ShowError(string message);


        MessageBoxResult Show(string message, string titlebarCaption);

        MessageBoxResult Show(string message, string titlebarCaption, MessageBoxButton button);

        MessageBoxResult Show(string message, string titlebarCaption, MessageBoxButton button, MessageBoxImage icon);

        MessageBoxResult ShowConfirmDelete();

        MessageBoxResult ShowConfirmYesNo(string message);
    }
}
