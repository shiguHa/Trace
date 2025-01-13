
using System.Windows.Input;

namespace Trace.Helpers
{
    public static class KeyboardHelper
    {
        public static bool IsCtrlKeyPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }
    }
}
