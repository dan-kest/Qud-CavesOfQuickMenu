namespace CavesOfQuickMenu.Concepts
{
    public static class TexturePath
    {
        private const string GENERAL = "CavesOfQuickMenu/QuickMenu/General";
        public const string GENERAL_LEGACY = GENERAL + "/Legacy/Slice {0}.png";
        public const string GENERAL_LEGACY_SELECTED = GENERAL + "/Legacy_{0}/Slice {1}.png";
    }

    public static class Book
    {
        public const string HELP = "Book_CavesOfQuickMenu_Help";
    }

    public static class Command
    {
        public const string OPEN_GENERAL = "Cmd_CavesOfQuickMenu_OpenGeneral";
        public const string CLOSE = "Cmd_CavesOfQuickMenu_Close";
    }

    public static class Screen
    {
        public const string GENERAL = "CavesOfQuickMenu:General";
    }
}
