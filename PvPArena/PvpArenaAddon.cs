using SLHub.Addons;
using Synapse;

namespace PvPArena
{
    public class PvpArenaAddon : IHubAddon
    {
        [AddonConfig("PvPArena")]
        public HubAddonConfig Config { get; set; }

        public void Load()
        {
            Server.Get.Events.Player.PlayerItemUseEvent += ItemUse;
        }

        public void NewRound()
        {
            foreach (var holder in Config.ItemHolders)
                new ItemHolder(holder.Item, holder.Teleporter);
        }

        public void Reload() { }

        private void ItemUse(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            if (Config.PvpRooms.Contains(ev.Player.Room.RoomType))
                ev.Allow = true;
        }
    }
}
