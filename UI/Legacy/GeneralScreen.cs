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
            int delay = QudOptions.NextScreenDelay;
            if (screenCode != QudScreenCode.None && delay > 0)
            {
                Thread.Sleep(delay);
            }
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
                string cmd = LegacyKeyMapping.GetCommandFromKey(input);
                // Exit
                if (InputUtil.IsCommand(cmd, "CmdSystemMenu", Command.OPEN_GENERAL, Command.CLOSE, "CmdCancel"))
                {
                    return ChangeScreen(QudScreenCode.None);
                }
                // Skills & Powers
                if (InputUtil.IsCommand(cmd, "CmdMoveN"))
                {
                    DrawSelected(Direction.N);
                    return ChangeScreen(QudScreenCode.Skills);
                }
                // Character Sheet
                if (InputUtil.IsCommand(cmd, "CmdMoveNE"))
                {
                    DrawSelected(Direction.NE);
                    return ChangeScreen(QudScreenCode.Character);
                }
                // Inventory
                if (InputUtil.IsCommand(cmd, "CmdMoveE"))
                {
                    DrawSelected(Direction.E);
                    return ChangeScreen(QudScreenCode.Inventory);
                }
                // Equipment
                if (InputUtil.IsCommand(cmd, "CmdMoveSE"))
                {
                    DrawSelected(Direction.SE);
                    return ChangeScreen(QudScreenCode.Equipment);
                }
                // Factions (Reputation)
                if (InputUtil.IsCommand(cmd, "CmdMoveS"))
                {
                    DrawSelected(Direction.S);
                    return ChangeScreen(QudScreenCode.Factions);
                }
                // Quests
                if (InputUtil.IsCommand(cmd, "CmdMoveSW"))
                {
                    DrawSelected(Direction.SW);
                    return ChangeScreen(QudScreenCode.Quests);
                }
                // Journal
                if (InputUtil.IsCommand(cmd, "CmdMoveW"))
                {
                    DrawSelected(Direction.W);
                    return ChangeScreen(QudScreenCode.Journal);
                }
                // Tinkering
                if (InputUtil.IsCommand(cmd, "CmdMoveNW"))
                {
                    DrawSelected(Direction.NW);
                    return ChangeScreen(QudScreenCode.Tinkering);
                }
                // Message History
                if (InputUtil.IsCommand(cmd, "CmdWait") || InputUtil.IsMouseEvent(input, "CmdWait"))
                {
                    DrawSelected(Direction.M);
                    return ChangeScreen(QudScreenCode.Message);
                }
                // Help
                if (InputUtil.IsCommand(cmd, "CmdHelp"))
                {
                    BookUI.ShowBook(Book.HELP);
                }
            }
        }
    }
}
