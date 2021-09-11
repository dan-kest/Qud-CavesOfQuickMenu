namespace CavesOfQuickMenu.Configs
{
    public static class META
    {
        public const string MOD_NAME = "CavesOfQuickMenu";
    }

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

    public static class BOOK
    {
        private const string prefix = "Book_" + META.MOD_NAME + "_";
        public const string STATUS_HELP = prefix + "Status_Help";
    }

    public static class COMMAND
    {
        private const string prefix = "Cmd_" + META.MOD_NAME + "_";
        public const string OPEN_STATUS = prefix + "Open_Status";
    }

    public static class SCREEN
    {
        private const string prefix = META.MOD_NAME + ":";
        public const string STATUS = prefix + "Status";
    }
}
