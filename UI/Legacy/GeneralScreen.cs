using System.Threading;
using ConsoleLib.Console;
using CavesOfQuickMenu.Utilities;
using CavesOfQuickMenu.Concepts;
namespace XRL.UI
{
    [UIView(Screen.GENERAL, false, false, false, "Adventure", null, false, 0, false)]
    public class GeneralScreen : IWantsTextConsoleInit
    {
        private static TextConsole _textConsole;
        private static ScreenBuffer _screenBuffer;

        private static ScreenBuffer buffer;
        private static ScreenBuffer bufferOld;

        private static int baseX1, baseY1, baseX2, baseY2;

        public void Init(TextConsole textConsole, ScreenBuffer screenBuffer)
		{
			_textConsole = textConsole;
			_screenBuffer = screenBuffer;

            (baseX1, baseY1, baseX2, baseY2) = LegacyCoord.GetBaseCoord();
		}

        private static void Draw()
        {
            if (buffer != null)
            {
                ConsoleUtil.SuppressScreenBufferImposters(true, baseX1, baseY1, baseX2, baseY2);
                buffer.Fill(baseX1, baseY1, baseX2, baseY2);
                int sliceCount = 1;
                for (int y = baseY1; y <= baseY2; y++)
                {
                    for (int x = baseX1; x <= baseX2; x++)
                    {
                        buffer.SetTileAt(x, y, TextureUtil.GetGeneralLegacy(sliceCount));
                        sliceCount++;
                    }
                }
                _textConsole.DrawBuffer(buffer);
            }
        }

        private static void DrawSelected(Direction direction)
        {
            if (buffer != null)
            {
                (int x1, int y1, int x2, int y2) = LegacyCoord.GetSelectedCoord(direction);
                int sliceCount = 1;
                for (int y = y1; y <= y2; y++)
                {
                    for (int x = x1; x <= x2; x++)
                    {
                        buffer.SetTileAt(x, y, TextureUtil.GetGeneralLegacySelected(direction, sliceCount));
                        sliceCount++;
                    }
                }
                _textConsole.DrawBuffer(buffer);
            }
        }

        private static void Erase()
        {
            if (buffer != null && bufferOld != null)
            {
                _textConsole.DrawBuffer(bufferOld);
                ConsoleUtil.SuppressScreenBufferImposters(false, baseX1, baseY1, baseX2, baseY2);
            }
        }

        private static QudScreenCode ChangeScreen(QudScreenCode screenCode)
        {
            Thread.Sleep(200); // High enough to prevent accidental input leak to the next screen
            Erase();
            GameManager.Instance.PopGameView();
            return screenCode;
        }

        public static QudScreenCode Show()
        {
            GameManager.Instance.PushGameView(Screen.GENERAL);
            TextConsole.LoadScrapBuffers();
            buffer = TextConsole.ScrapBuffer;
            bufferOld = TextConsole.ScrapBuffer2;
            Draw();
            while (true)
            {
                Keys input = Keyboard.getvk(false);
                // Exit
                if (InputUtil.IsMouseEvent(input, "CmdSystemMenu", Command.OPEN_GENERAL, "Cancel"))
                {
                    return ChangeScreen(QudScreenCode.None);
                }
                // Skills & Powers
                if (InputUtil.IsMouseEvent(input, "CmdMoveN"))
                {
                    DrawSelected(Direction.N);
                    return ChangeScreen(QudScreenCode.Skills);
                }
                // Character Sheet
                if (InputUtil.IsMouseEvent(input, "CmdMoveNE"))
                {
                    DrawSelected(Direction.NE);
                    return ChangeScreen(QudScreenCode.Character);
                }
                // Inventory
                if (InputUtil.IsMouseEvent(input, "CmdMoveE"))
                {
                    DrawSelected(Direction.E);
                    return ChangeScreen(QudScreenCode.Inventory);
                }
                // Equipment
                if (InputUtil.IsMouseEvent(input, "CmdMoveSE"))
                {
                    DrawSelected(Direction.SE);
                    return ChangeScreen(QudScreenCode.Equipment);
                }
                // Factions (Reputation)
                if (InputUtil.IsMouseEvent(input, "CmdMoveS"))
                {
                    DrawSelected(Direction.S);
                    return ChangeScreen(QudScreenCode.Factions);
                }
                // Quests
                if (InputUtil.IsMouseEvent(input, "CmdMoveSW"))
                {
                    DrawSelected(Direction.SW);
                    return ChangeScreen(QudScreenCode.Quests);
                }
                // Journal
                if (InputUtil.IsMouseEvent(input, "CmdMoveW"))
                {
                    DrawSelected(Direction.W);
                    return ChangeScreen(QudScreenCode.Journal);
                }
                // Tinkering
                if (InputUtil.IsMouseEvent(input, "CmdMoveNW"))
                {
                    DrawSelected(Direction.NW);
                    return ChangeScreen(QudScreenCode.Tinkering);
                }
                // Message History
                if (InputUtil.IsMouseEvent(input, "CmdWait"))
                {
                    DrawSelected(Direction.M);
                    return ChangeScreen(QudScreenCode.Message);
                }
            }
        }
    }
}
