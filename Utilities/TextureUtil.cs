using CavesOfQuickMenu.Concepts;

namespace CavesOfQuickMenu.Utilities
{
    public static class TextureUtil
    {
        public static string GetStatusQuickMenuTexture(int no)
        {
            return TEXTURE_PATH.STATUS_QUICK_MENU_LEGACY.Replace("{{1}}", no.ToString());
        }
    }
}
