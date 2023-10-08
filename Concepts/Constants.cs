namespace CavesOfQuickMenu.Concepts
{
    public static class QUD_SCREEN_CODE
    {
        public const int SKILLS    = 0;
        public const int CHARACTER = 1;
        public const int INVENTORY = 2;
        public const int EQUIPMENT = 3;
        public const int FACTIONS  = 4;
        public const int QUESTS    = 5;
        public const int JOURNAL   = 6;
        public const int TINKERING = 7;
    }

    public static class QUICK_MENU_SCREEN_CODE
    {
        public const int NONE      = -1000;
        public const int MESSAGE   = 1000;
        public const int EFFECTS   = 1001;
        public const int ABILITIES = 1002;
    }

    public static class QUICK_MENU_LEGACY_COORD
    {
        public const int X1 = 24;
        public const int Y1 = 2;
        public const int X2 = 55;
        public const int Y2 = 22;
    }

    public static class TEXTURE_PATH
    {
        public const string GENERAL_LEGACY = "CavesOfQuickMenu/QuickMenu/General/Legacy/Slice {{1}}.png";
    }

    public static class BOOK
    {
        public const string HELP = "Book_CavesOfQuickMenu_Help";
    }

    public static class COMMAND
    {
        public const string OPEN_GENERAL = "Cmd_CavesOfQuickMenu_OpenGeneral";
    }

    public static class SCREEN
    {
        public const string GENERAL = "CavesOfQuickMenu:General";
    }
}
