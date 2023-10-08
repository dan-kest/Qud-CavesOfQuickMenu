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

        private static void Erase()
        {
            if (buffer != null && bufferOld != null)
            {
                _textConsole.DrawBuffer(bufferOld);
                ConsoleUtil.SuppressScreenBufferImposters(false, X1, Y1, X2, Y2);
            }
        }

        private static int ChangeScreen(int screenCode)
        {
            Erase();
            GameManager.Instance.PopGameView();
            return screenCode;
        }

        public static int Show()
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
                    return ChangeScreen(QUICK_MENU_SCREEN_CODE.NONE);
                }
                // Skills & Powers
                if (InputUtil.IsMouseEvent(input, "CmdMoveN"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.SKILLS);
                }
                // Character Sheet
                if (InputUtil.IsMouseEvent(input, "CmdMoveNE"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.CHARACTER);
                }
                // Inventory
                if (InputUtil.IsMouseEvent(input, "CmdMoveE"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.INVENTORY);
                }
                // Equipment
                if (InputUtil.IsMouseEvent(input, "CmdMoveSE"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.EQUIPMENT);
                }
                // Factions (Reputation)
                if (InputUtil.IsMouseEvent(input, "CmdMoveS"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.FACTIONS);
                }
                // Quests
                if (InputUtil.IsMouseEvent(input, "CmdMoveSW"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.QUESTS);
                }
                // Journal
                if (InputUtil.IsMouseEvent(input, "CmdMoveW"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.JOURNAL);
                }
                // Tinkering
                if (InputUtil.IsMouseEvent(input, "CmdMoveNW"))
                {
                    return ChangeScreen(QUD_SCREEN_CODE.TINKERING);
                }
                // Message History
                if (InputUtil.IsMouseEvent(input, "CmdWait"))
                {
                    return ChangeScreen(QUICK_MENU_SCREEN_CODE.MESSAGE);
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
