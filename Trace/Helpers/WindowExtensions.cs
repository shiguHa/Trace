using System.Windows;
using System.Windows.Controls;

namespace Trace.Helpers
{
    internal static class WindowExtensions
    {
        public static object GetDataContext(this Window window)
        {
            if (window.Content is Frame frame)
            {
                return frame.GetDataContext();
            }

            return null;
        }
    }
}
