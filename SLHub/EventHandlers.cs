using MEC;
using Synapse;
using Synapse.Api;
using System.Collections.Generic;
using System.Linq;

namespace SLHub
{
    public class EventHandlers
    {
        public readonly List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        private PluginClass Plugin { get; }

        public CustomPluginConfig Config { get; set; }

        public EventHandlers(PluginClass pluginClass)
        {
            Plugin = pluginClass;
            Reload();

            Server.Get.Events.Round.WaitingForPlayersEvent += Waiting;
            Server.Get.Events.Player.PlayerDropItemEvent += Drop;
            Server.Get.Events.Round.RoundRestartEvent += Restart;
            Server.Get.Events.Player.PlayerLeaveEvent += Leave;
            Server.Get.Events.Player.PlayerDeathEvent += Death;
            Server.Get.Events.Player.PlayerEscapesEvent += Escape;
            Server.Get.Events.Player.PlayerSetClassEvent += SetClass;
            Server.Get.Events.Player.PlayerDropAmmoEvent += DropAmmo;
        }

        private void SetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev)
        {
            if (!Config.SpawnWithItems)
                ev.Items.Clear();
        }

        public void Reload() => Config = Plugin.SYML.GetOrSetDefault("LobbyConfigs", new CustomPluginConfig());

        private IEnumerator<float> Lobby()
        {
            for(; ; )
            {
                yield return MEC.Timing.WaitForSeconds(0.2f);

                foreach (var player in RoleType.Spectator.GetPlayers())
                    player.RoleType = Config.SpawnRole;

                if (Config.DestroyAllRagdolls)
                    foreach (var rag in Map.Get.Ragdolls.ToArray())
                        rag.Destroy();
            }
        }

        private void Escape(Synapse.Api.Events.SynapseEventArguments.PlayerEscapeEventArgs ev)
        {
            if (!Config.AllowEscape)
                ev.Allow = false;
        }

        private void Waiting()
        {
            Map.Get.Round.RoundLock = true;
            Map.Get.RespawnPoint = Config.SpawnPoint.Parse().Position;
            Map.Get.Round.StartRound();

            if (Config.LockAllDoors) Map.Get.Doors.ForEach(x => x.Locked = true);
            if (Config.LockAllElevators) Map.Get.Elevators.ForEach(x => x.Locked = true);
            Map.Get.Doors.Where(x => Config.LockedDoors.Contains(x.DoorType)).ToList().ForEach(x => x.Locked = true);
            Map.Get.Doors.Where(x => Config.OpenDoors.Contains(x.DoorType)).ToList().ForEach(x => x.Open = true);
            Map.Get.Elevators.Where(x => Config.LockedElevators.Contains(x.ElevatorType)).ToList().ForEach(x => x.Locked = true);

            coroutines.Add(Timing.RunCoroutine(Lobby()));
        }

        private void Restart()
        {
            Timing.KillCoroutines(coroutines.ToArray());
            coroutines.Clear();
        }

        private void Drop(Synapse.Api.Events.SynapseEventArguments.PlayerDropItemEventArgs ev)
        {
            if (!Config.AllowDrop)
            {
                ev.Allow = false;
                ev.Item.Destroy();
            }
        }

        private void DropAmmo(Synapse.Api.Events.SynapseEventArguments.PlayerDropAmmoEventArgs ev)
        {
            if (!Config.AllowDrop)
                ev.Allow = false;
        }

        private void Leave(Synapse.Api.Events.SynapseEventArguments.PlayerLeaveEventArgs ev)
        {
            foreach (var item in ev.Player.Inventory.Items)
                item.Destroy();
        }

        private void Death(Synapse.Api.Events.SynapseEventArguments.PlayerDeathEventArgs ev) => ev.Victim.Inventory.Clear();
    }
}
