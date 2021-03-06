using System.Collections.Generic;
using ConsoleLib.Console;
using XRL.UI;

namespace CavesOfQuickMenu.Utilities
{
    public static class InputUtilities
    {
        public static (int, int) GetAllKeysFromCommand(string Cmd)
        {
            int primary = 0;
            int secondary = 0;
            foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in LegacyKeyMapping.CurrentMap.PrimaryMapCommandToKeyLayer)
            {
                if (keyValuePair.Value.TryGetValue(Cmd, out primary))
                {
                    break;
                }
            }
            foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair2 in LegacyKeyMapping.CurrentMap.SecondaryMapCommandToKeyLayer)
            {
                if (keyValuePair2.Value.TryGetValue(Cmd, out secondary))
                {
                    break;
                }
            }
            return (primary, secondary);
        }

        private const int Shift = (int) Keys.Shift;
        private const int Control = (int) Keys.Control;
        private const int Alt = (int) Keys.Alt;
        private const int ShiftControl = Shift + Control;
        private const int ShiftAlt = Shift + Alt;
        private const int ControlAlt = Control + Alt;
        private const int ShiftControlAlt = ShiftControl + Alt;

        public static Keys GetShift(Keys keys)
        {
            return (Keys) keys + Shift;
        }

        public static Keys GetControl(Keys keys)
        {
            return (Keys) keys + Control;
        }

        public static Keys GetAlt(Keys keys)
        {
            return (Keys) keys + Alt;
        }

        public static bool HasAnyModifiers(Keys input, Keys keys)
        {
            return input == keys + Shift || input == keys + Control || input == keys + Alt
                    || input == keys + ShiftControl || input == keys + ShiftAlt
                    || input == keys + ControlAlt || input == keys + ShiftControlAlt;
        }
    }
}
