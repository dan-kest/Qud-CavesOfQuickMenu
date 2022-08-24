using CavesOfQuickMenu.Concepts;

namespace CavesOfQuickMenu.Utilities
{
    public static class TextureUtil
    {
        public static string GetQuickMenuGeneralLegacyTexture(int no)
        {
            return TEXTURE_PATH.QUICK_MENU_GENERAL_LEGACY.Replace("{{1}}", no.ToString());
        }
    }
}
