using Synapse;

namespace SLHub.Teleporter
{
    public class TeleporterHandler
    {
        private PluginClass Plugin { get; }

        public TeleporterConfig Config { get; private set; }

        internal TeleporterHandler(PluginClass pluginClass)
        {
            Plugin = pluginClass;
            Reload();

            Server.Get.Events.Round.WaitingForPlayersEvent += Waiting;
        }

        public void Reload() => Config = Plugin.SYML.GetOrSetDefault("Teleporter", new TeleporterConfig());

        private void Waiting()
        {
            foreach (var tele in Config.Teleporters)
                new Teleporter(tele.Position.Parse().Position, tele.LookingAt.Parse().Position, tele.TeleportTo.Parse().Position, tele.Role, tele.Name);
        }
    }
}
