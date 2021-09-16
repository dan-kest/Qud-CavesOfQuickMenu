using ConsoleLib.Console;
using CavesOfQuickMenu.Utilities;
using CavesOfQuickMenu.Configs;

namespace XRL.UI
{
    public class CavesOfQuickMenu_StatusQuickMenuScreen
    {
        private const int X1 = QUICK_MENU_LEGACY_COORD.X1;
        private const int Y1 = QUICK_MENU_LEGACY_COORD.Y1;
        private const int X2 = QUICK_MENU_LEGACY_COORD.X2;
        private const int Y2 = QUICK_MENU_LEGACY_COORD.Y2;

        private static ScreenBuffer Buffer;
        private static ScreenBuffer OldBuffer;

        private static void Draw()
        {
            if (Buffer != null)
            {
                ConsoleUtilities.SuppressScreenBufferImposters(true, X1, Y1, X2, Y2);
                Buffer.Fill(X1, Y1, X2, Y2);
                int sliceCount = 1;
                for (int y = Y1; y <= Y2; y++)
                {
                    for (int x = X1; x <= X2; x++)
                    {
                        Buffer.SetTileAt(x, y, TextureUtilities.GetStatusQuickMenuTexture(sliceCount));
                        sliceCount++;
                    }
                }
                Popup._TextConsole.DrawBuffer(Buffer);
            }
        }

        private static void Erase()
        {
            if (Buffer != null && OldBuffer != null)
            {
                Popup._TextConsole.DrawBuffer(OldBuffer);
                ConsoleUtilities.SuppressScreenBufferImposters(false, X1, Y1, X2, Y2);
            }
        }

        public static int Show()
        {
            GameManager.Instance.PushGameView(SCREEN.STATUS);
            TextConsole.LoadScrapBuffers();
            Buffer = TextConsole.ScrapBuffer;
            OldBuffer = TextConsole.ScrapBuffer2;
            Draw();
            while (true)
            {
                Keys input = Keyboard.getvk(false);
                string cmd = LegacyKeyMapping.GetCommandFromKey(input);
                // Exit
                if (input == Keys.Escape || cmd == COMMAND.OPEN_STATUS || cmd == "CmdCancel")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUICK_MENU_SCREEN_CODE.NONE;
                }
                // Skills & Powers
                if (cmd == "CmdMoveN")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.SKILLS;
                }
                // Character Sheet
                if (cmd == "CmdMoveNE")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.CHARACTER;
                }
                // Inventory
                if (cmd == "CmdMoveE")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.INVENTORY;
                }
                // Equipment
                if (cmd == "CmdMoveSE")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.EQUIPMENT;
                }
                // Factions (Reputation)
                if (cmd == "CmdMoveS")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.FACTIONS;
                }
                // Quests
                if (cmd == "CmdMoveSW")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.QUESTS;
                }
                // Journal
                if (cmd == "CmdMoveW")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.JOURNAL;
                }
                // Tinkering
                if (cmd == "CmdMoveNW")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUD_SCREEN_CODE.TINKERING;
                }
                // Message History
                if (cmd == "CmdWait")
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUICK_MENU_SCREEN_CODE.MESSAGE;
                }
                // Abilities
                (int keyCmdMoveN1, int keyCmdMoveN2) = InputUtilities.GetAllKeysFromCommand("CmdMoveN");
                if (InputUtilities.HasAnyModifiers(input, (Keys) keyCmdMoveN1) || InputUtilities.HasAnyModifiers(input, (Keys) keyCmdMoveN2)
                        || InputUtilities.HasAnyModifiers(input, Keys.Up))
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUICK_MENU_SCREEN_CODE.ABILITIES;
                }
                // Active Effects
                (int keyCmdMoveNE1, int keyCmdMoveNE2) = InputUtilities.GetAllKeysFromCommand("CmdMoveNE");
                if (InputUtilities.HasAnyModifiers(input, (Keys) keyCmdMoveNE1) || InputUtilities.HasAnyModifiers(input, (Keys) keyCmdMoveNE2))
                {
                    Erase();
                    GameManager.Instance.PopGameView();
                    return QUICK_MENU_SCREEN_CODE.EFFECTS;
                }
                // Help
                if (input == InputUtilities.GetShift(Keys.OemQuestion) || input == Keys.F1)
                {
                    BookUI.ShowBook(BOOK.STATUS_HELP, null);
                }
            }
        }
    }
}
