using System.Collections.Generic;
using ConsoleLib.Console;
using XRL.UI;

namespace CavesOfQuickMenu.Utilities
{
    public static class InputUtil
    {
        public static bool IsMouseEvent(Keys input, params string[] mouseEventNameList)
        {
            if (input != Keys.MouseEvent)
            {
                return false;
            }
            foreach (string mouseEventName in mouseEventNameList)
            {
                if (Keyboard.CurrentMouseEvent.Event == $"Command:{mouseEventName}")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
