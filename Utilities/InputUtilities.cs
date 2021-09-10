using ConsoleLib.Console;

namespace CavesOfQuickMenu.Utilities
{
    public static class InputUtilities
    {
        private const int Shift = (int) Keys.Shift;
        private const int Control = (int) Keys.Control;
        private const int Alt = (int) Keys.Alt;
        private const int ShiftControl = Shift + Control;
        private const int ShiftAlt = Shift + Alt;
        private const int ControlAlt = Control + Alt;
        private const int ShiftControlAlt = ShiftControl + Alt;

        public static Keys GetShift(Keys keys)
        {
            return (Keys) ((int) keys + Shift);
        }

        public static Keys GetControl(Keys keys)
        {
            return (Keys) ((int) keys + Control);
        }

        public static Keys GetAlt(Keys keys)
        {
            return (Keys) ((int) keys + Alt);
        }

        public static bool HasAnyModifiers(Keys input, Keys keys)
        {
            return input == keys + Shift || input == keys + Control || input == keys + Alt
                    || input == keys + ShiftControl || input == keys + ShiftAlt
                    || input == keys + ControlAlt || input == keys + ShiftControlAlt;
        }
    }
}
