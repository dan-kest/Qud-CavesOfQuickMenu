using System;
using XRL.UI;
using CavesOfQuickMenu.Concepts;

namespace XRL.World.Parts
{
    [Serializable]
    public class CavesOfQuickMenu_CommandListener : IPart
    {
        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, COMMAND.OPEN_STATUS);
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == COMMAND.OPEN_STATUS)
            {
                int screenCode = CavesOfQuickMenu_StatusQuickMenuScreen.Show();

                // Normal screen code range
                if (screenCode >= QUD_SCREEN_CODE.SKILLS && screenCode <= QUD_SCREEN_CODE.TINKERING)
                {
                    Screens.CurrentScreen = screenCode;
                    Screens.Show(The.Player);
                }
                // Message History
                else if (screenCode == QUICK_MENU_SCREEN_CODE.MESSAGE)
                {
                    The.Game.Player.Messages.Show();
                }
                // Abilities
                else if (screenCode == QUICK_MENU_SCREEN_CODE.ABILITIES)
                {
                    string command = AbilityManager.Show(The.Player);
                    if (!string.IsNullOrEmpty(command))
                    {
                        CommandEvent.Send(The.Player, command, null, null, null);
                    }
                }
                // Active Effects
                else if (screenCode == QUICK_MENU_SCREEN_CODE.EFFECTS)
                {
                    The.Player.ShowActiveEffects();
                }
            }
            return base.FireEvent(E);
        }
    }
}
