using XRL;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;

namespace CavesOfQuickMenu.Handlers
{
    [PlayerMutator]
    public class NewGameHandler : IPlayerMutator
    {
        public void mutate(GameObject player)
        {
            player.AddPart<CommandListener>();
            Hacks.ForceEnableAbility();
        }
    }

    [HasCallAfterGameLoadedAttribute]
    public class LoadGameHandler
    {
        [CallAfterGameLoadedAttribute]
        public static void LoadGameCallback()
        {
            GameObject player = XRLCore.Core?.Game?.Player?.Body;
            if (player != null)
            {
                player.RequirePart<CommandListener>();
                Hacks.ForceEnableAbility();
            }
        }
    }
}
