using System.Threading;
using ConsoleLib.Console;
using CavesOfQuickMenu.Utilities;
using CavesOfQuickMenu.Concepts;
namespace XRL.UI
{
    [UIView(SCREEN.GENERAL, false, false, false, "Adventure", null, false, 0, false)]
    public class GeneralScreen : IWantsTextConsoleInit
    {
        private const int X1 = QUICK_MENU_LEGACY_COORD.X1;
        private const int Y1 = QUICK_MENU_LEGACY_COORD.Y1;
        private const int X2 = QUICK_MENU_LEGACY_COORD.X2;
        private const int Y2 = QUICK_MENU_LEGACY_COORD.Y2;

        private static TextConsole _textConsole;
        private static ScreenBuffer _screenBuffer;

        private static ScreenBuffer buffer;
        private static ScreenBuffer bufferOld;

        public void Init(TextConsole textConsole, ScreenBuffer screenBuffer)
		{
			_textConsole = textConsole;
			_screenBuffer = screenBuffer;
		}

        private static void Draw()
        {
            if (buffer != null)
            {
                ConsoleUtil.SuppressScreenBufferImposters(true, X1, Y1, X2, Y2);
                buffer.Fill(X1, Y1, X2, Y2);
                int sliceCount = 1;
                for (int y = Y1; y <= Y2; y++)
                {
                    for (int x = X1; x <= X2; x++)
                    {
                        buffer.SetTileAt(x, y, TextureUtil.GetQuickMenuGeneralLegacyTexture(sliceCount));
                        sliceCount++;
                    }
                }
                _textConsole.DrawBuffer(buffer);
            }
        }

        private static void DrawSelected(Direction direction)
        {
        }

        private static void Erase()
        {
            if (buffer != null && bufferOld != null)
            {
                _textConsole.DrawBuffer(bufferOld);
                ConsoleUtil.SuppressScreenBufferImposters(false, X1, Y1, X2, Y2);
            }
        }

        private static QudScreenCode ChangeScreen(QudScreenCode screenCode)
        {
            Thread.Sleep(100); // Prevent input leak to the next screen
            Erase();
            GameManager.Instance.PopGameView();
            return screenCode;
        }

        public static QudScreenCode Show()
        {
            GameManager.Instance.PushGameView(SCREEN.GENERAL);
            TextConsole.LoadScrapBuffers();
            buffer = TextConsole.ScrapBuffer;
            bufferOld = TextConsole.ScrapBuffer2;
            Draw();
            while (true)
            {
                Keys input = Keyboard.getvk(false);
                // Exit
                if (InputUtil.IsMouseEvent(input, "CmdSystemMenu", COMMAND.OPEN_GENERAL, "Cancel"))
                {
                    return ChangeScreen(QudScreenCode.None);
                }
                // Skills & Powers
                if (InputUtil.IsMouseEvent(input, "CmdMoveN"))
                {
                    return ChangeScreen(QudScreenCode.Skills);
                }
                // Character Sheet
                if (InputUtil.IsMouseEvent(input, "CmdMoveNE"))
                {
                    return ChangeScreen(QudScreenCode.Character);
                }
                // Inventory
                if (InputUtil.IsMouseEvent(input, "CmdMoveE"))
                {
                    return ChangeScreen(QudScreenCode.Inventory);
                }
                // Equipment
                if (InputUtil.IsMouseEvent(input, "CmdMoveSE"))
                {
                    return ChangeScreen(QudScreenCode.Equipment);
                }
                // Factions (Reputation)
                if (InputUtil.IsMouseEvent(input, "CmdMoveS"))
                {
                    return ChangeScreen(QudScreenCode.Factions);
                }
                // Quests
                if (InputUtil.IsMouseEvent(input, "CmdMoveSW"))
                {
                    return ChangeScreen(QudScreenCode.Quests);
                }
                // Journal
                if (InputUtil.IsMouseEvent(input, "CmdMoveW"))
                {
                    return ChangeScreen(QudScreenCode.Journal);
                }
                // Tinkering
                if (InputUtil.IsMouseEvent(input, "CmdMoveNW"))
                {
                    return ChangeScreen(QudScreenCode.Tinkering);
                }
                // Message History
                if (InputUtil.IsMouseEvent(input, "CmdWait"))
                {
                    return ChangeScreen(QudScreenCode.Message);
                }

                // ===== Alternate Manu =====
                // (int keyCmdAltMenu1, int keyCmdAltMenu2) = InputUtil.GetAllKeysFromCommand(COMMAND.ALT_MENU);

                // Abilities
                // (int keyCmdMoveN1, int keyCmdMoveN2) = InputUtil.GetAllKeysFromCommand("CmdMoveN");
                // if (InputUtil.HasAnyModifiers(input, (Keys) keyCmdMoveN1) || InputUtil.HasAnyModifiers(input, (Keys) keyCmdMoveN2)
                //         || InputUtil.HasAnyModifiers(input, Keys.Up))
                // {
                //     Erase();
                //     GameManager.Instance.PopGameView();
                //     return QUICK_MENU_GENERAL_SCREEN_CODE.ABILITIES;
                // }
                // // Active Effects
                // (int keyCmdMoveNE1, int keyCmdMoveNE2) = InputUtil.GetAllKeysFromCommand("CmdMoveNE");
                // if (InputUtil.HasAnyModifiers(input, (Keys) keyCmdMoveNE1) || InputUtil.HasAnyModifiers(input, (Keys) keyCmdMoveNE2))
                // {
                //     Erase();
                //     GameManager.Instance.PopGameView();
                //     return QUICK_MENU_GENERAL_SCREEN_CODE.EFFECTS;
                // }
                // // Help
                // if (input == InputUtil.GetShift(Keys.OemQuestion) || input == Keys.F1)
                // {
                //     BookUI.ShowBook(BOOK.HELP, null);
                // }
            }
        }
    }
}
