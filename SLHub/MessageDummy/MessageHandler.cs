using Synapse;

namespace SLHub.MessageDummy
{
    public class MessageHandler
    {
        internal MessageHandler(PluginClass plugin)
        {
            Plugin = plugin;
            Reload();

            Server.Get.Events.Round.WaitingForPlayersEvent += Waiting;
        }

        public MessageConfig Config { get; private set; }

        public PluginClass Plugin { get; }

        public void Reload() => Config = Plugin.SYML.GetOrSetDefault("MessageDummies", new MessageConfig());

        private void Waiting()
        {
            foreach (var dummy in Config.MessageDummies)
                new MessageDummy(dummy);
        }
    }
}
