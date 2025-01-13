
using System.Windows;
using Trace.Interfaces;

namespace Trace.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message)
        {
            return MessageBox.Show(message);
        }

        public MessageBoxResult Show(string message, string titlebarCaption)
        {
            return MessageBox.Show(message, titlebarCaption);
        }

        public MessageBoxResult Show(string message, string titlebarCaption, MessageBoxButton button)
        {
            return MessageBox.Show(message, titlebarCaption, button);
        }

        public MessageBoxResult Show(string message, string titlebarCaption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(message, titlebarCaption, button, icon);
        }

        public MessageBoxResult ShowError(string message)
        {
            return Show(message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MessageBoxResult ShowConfirmDelete()
        {
            return MessageBox.Show("本当に削除していいですか？",
                                                            "確認",
                                                            System.Windows.MessageBoxButton.YesNo,
                                                            System.Windows.MessageBoxImage.Question);
        }

        public MessageBoxResult ShowConfirmYesNo(string message)
        {
            return MessageBox.Show(message,
                                                "確認",
                                                System.Windows.MessageBoxButton.YesNo,
                                                System.Windows.MessageBoxImage.Question);
        }
    }
}
