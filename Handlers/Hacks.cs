using System.Collections.Generic;
using CavesOfQuickMenu.Concepts;

namespace CavesOfQuickMenu.Handlers
{
    public class Hacks
    {
        // Hack for 9 Feb 2024 update that blocks abilities on the world map
        // Thanks HunterZ for this work around (I just copy-pasted his code lol)
        private static HarmonyLib.Traverse WorldMapAllowed = null;
        public static void ForceEnableAbility()
        {
            WorldMapAllowed ??= HarmonyLib.Traverse.Create(typeof(XRL.UI.AbilityManager))?.Field("WorldMapAllowed");
            if (null == WorldMapAllowed) { return; }
            List<string> list = WorldMapAllowed.GetValue<List<string>>();
            bool newList = false;
            if (null == list)
            {
                newList = true;
                list = new List<string>();
            }
            if (!list.Contains(QudCommand.OPEN_GENERAL))
            {
                list.Add(QudCommand.OPEN_GENERAL);
            }
            if (!list.Contains(QudCommand.CLOSE))
            {
                list.Add(QudCommand.CLOSE);
            }
            if (newList)
            {
                WorldMapAllowed.SetValue(list);
            }
        }
    }
}
