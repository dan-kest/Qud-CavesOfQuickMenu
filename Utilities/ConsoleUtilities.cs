using ConsoleLib.Console;

namespace CavesOfQuickMenu.Utilities
{
    public static class ConsoleUtilities
    {
        public static void Fill(this ScreenBuffer buffer, int x1, int y1, int x2, int y2)
        {
            buffer.Fill(x1, y1, x2, y2, 32, ColorUtility.MakeColor(TextColor.Grey, TextColor.Black));
        }

        public static void SingleBox(this ScreenBuffer buffer, int x1 = 0, int y1 = 0, int x2 = 79, int y2 = 24, bool isFill = true)
        {
            if (isFill)
            {
                buffer.Fill(x1, y1, x2, y2);
            }
            buffer.SingleBox(x1, y1, x2, y2, ColorUtility.MakeColor(TextColor.Grey, TextColor.Black));
        }

        public static void Title(this ScreenBuffer buffer, string title, bool isFormat = true)
        {
            if (isFormat)
            {
                title = "{{y|[ {{W|" + title + "}} ]}}";
            }
            int startPos = (80 - ColorUtility.StripFormatting(title).Length) / 2;
            buffer.Goto(startPos, 0);
            buffer.Write(title);
        }

        public static void SetDefaultTextColor(this ConsoleChar consoleChar)
        {
            consoleChar.SetBackground('k');
            consoleChar.SetForeground('y');
        }

        public static void SetCharAt(this ScreenBuffer buffer, int x, int y, char c)
        {
            buffer[x, y].Char = c;
            buffer[x, y].SetDefaultTextColor();
        }

        public static void SetTileAt(this ScreenBuffer buffer, int x, int y, string path)
        {
            buffer[x, y].Tile = path;
        }

        public static void SuppressScreenBufferImposters(bool isSuppress = true, int x1 = 0, int y1 = 0, int x2 = 79, int y2 = 24)
        {
            for (int y = y1; y <= y2; y++)
            {
                for (int x = x1; x <= x2; x++)
                {
                    ScreenBuffer.ImposterSuppression[x, y] = isSuppress;
                }
            }
        }
    }
}
