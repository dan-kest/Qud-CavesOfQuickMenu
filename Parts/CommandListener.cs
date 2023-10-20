using System;
using XRL.UI;
using CavesOfQuickMenu.Concepts;

namespace XRL.World.Parts
{
    [Serializable]
    public class CommandListener : IPart
    {
        public override void Register(GameObject obj)
        {
            obj.RegisterPartEvent(this, QudCommand.OPEN_GENERAL);
            base.Register(obj);
        }

        public override bool FireEvent(Event e)
        {
            if (e.ID == QudCommand.OPEN_GENERAL)
            {
                QudScreenCode screenCode = GeneralScreen.Show();

                // Normal screen code range
                if (screenCode >= QudScreenCode.Skills && screenCode <= QudScreenCode.Tinkering)
                {
                    Screens.CurrentScreen = (int) screenCode;
                    Screens.Show(The.Player);
                }
                // Message History
                else if (screenCode == QudScreenCode.Message)
                {
                    The.Game.Player.Messages.Show();
                }
                // Abilities
                // else if (screenCode == QudScreenCode.Abilities)
                // {
                //     string command = AbilityManager.Show(The.Player);
                //     if (!string.IsNullOrEmpty(command))
                //     {
                //         CommandEvent.Send(The.Player, command);
                //     }
                // }
                // Active Effects
                // else if (screenCode == QudScreenCode.Effects)
                // {
                //     The.Player.ShowActiveEffects();
                // }
            }
            return base.FireEvent(e);
        }

        public override bool WantEvent(int id, int cascade)
        {
            return base.WantEvent(id, cascade) || id == AfterPlayerBodyChangeEvent.ID;
        }

        public override bool HandleEvent(AfterPlayerBodyChangeEvent e)
        {
            if (e.OldBody == ParentObject)
            {
                e.OldBody.RemovePart(this);
            }
            if (e.NewBody != null && !e.NewBody.HasPart<CommandListener>())
            {
                e.NewBody.AddPart(this);
            }
            return base.HandleEvent(e);
        }
    }
}
