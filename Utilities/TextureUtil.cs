using System;
using CavesOfQuickMenu.Concepts;

namespace CavesOfQuickMenu.Utilities
{
    public static class TextureUtil
    {
        public static string GetGeneralLegacy(int no)
        {
            return String.Format(TexturePath.GENERAL_LEGACY, no.ToString());
        }

        public static string GetGeneralLegacySelected(Direction direction, int no)
        {
            return String.Format(TexturePath.GENERAL_LEGACY_SELECTED, direction, no.ToString());
        }
    }
}
