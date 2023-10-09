using System;

namespace CavesOfQuickMenu.Concepts
{
    public class QudOptions
    {
        private static string GetOption(string id)
        {
            return XRL.UI.Options.GetOption(id);
        }

        public static int NextScreenDelay => Convert.ToInt32(GetOption("Option_CavesOfQuickMenu_NextScreenDelay"));
    }
}