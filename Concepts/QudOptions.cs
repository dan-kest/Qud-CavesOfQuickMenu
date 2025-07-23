using System;

namespace CavesOfQuickMenu.Concepts
{
    public class QudOption
    {
        private static string GetOption(string id)
        {
            return XRL.UI.Options.GetOption(id);
        }

        public static bool IsForceFullscreen => GetOption("Option_CavesOfQuickMenu_IsForceFullscreen").EqualsNoCase("Yes");
        public static int WaitInputReleaseTimeout => Convert.ToInt32(GetOption("Option_CavesOfQuickMenu_WaitInputReleaseTimeout"));
        public static float DeadzoneThreshold = 0.4f;
        public static int InputInterval = 10;
    }
}