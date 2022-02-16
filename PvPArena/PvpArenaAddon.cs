using SLHub.Addons;
using Synapse;

namespace PvPArena
{
    public class PvpArenaAddon : IHubAddon
    {
        [AddonConfig("PvPArena")]
        public HubAddonConfig Config { get; set; }

        public void Reload() { }

        public void Load()
        {
            Server.Get.Events.Player.PlayerShootEvent += Shoot;
            Server.Get.Events.Player.PlayerThrowGrenadeEvent += Throw;
            Server.Get.Events.Player.PlayerUseMicroEvent += Micro;
        }

        private void Micro(Synapse.Api.Events.SynapseEventArguments.PlayerUseMicroEventArgs ev)
        {
            if (!Config.PvpRooms.Contains(ev.Player.Room.RoomType))
                ev.State = InventorySystem.Items.MicroHID.HidState.Idle;
        }

        private void Throw(Synapse.Api.Events.SynapseEventArguments.PlayerThrowGrenadeEventArgs ev)
        {
            if (!Config.PvpRooms.Contains(ev.Player.Room.RoomType))
                ev.Allow = false;
        }

        private void Shoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if (!Config.PvpRooms.Contains(ev.Player.Room.RoomType))
                ev.Allow = false;
        }

        public void NewRound()
        {
            foreach (var holder in Config.ItemHolders)
                new ItemHolder(holder.Item, holder.Teleporter);
        }
    }
}
