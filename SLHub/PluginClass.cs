using Synapse.Api.Plugin;
using Synapse.Config;
using Synapse;
using System.IO;
using SLHub.Addons;
using SLHub.ServerTeleporter;
using SLHub.Interactable;
using SLHub.Teleporter;
using SLHub.MessageDummy;

namespace SLHub
{
    [PluginInformation(
        Name = "SLHub",
        Author = "Dimenzio",
        Description = "A Plugin that allows to create a Hub Server in SL",
        LoadPriority = 0,
        SynapseMajor = 2,
        SynapseMinor = 5,
        SynapsePatch = 2,
        Version = "v.1.0.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        public static PluginClass Singleton { get; private set; }

        public MessageHandler MessageHandler { get; private set; }

        public ServerHandler ServerHandler { get; private set; }

        public TeleporterHandler TeleporterHandler { get; private set; }

        public AddonHandler AddonHandler { get; private set; }

        public EventHandlers EventHandlers { get; set; }

        public HubDummyHandler DummyHandler { get; set; }

        public SYML SYML { get; private set; }

        public override void Load()
        {
            Singleton = this;

            //Setting up the syml
            var path = Path.Combine(Server.Get.Files.ConfigDirectory, "HubConfiguration.syml");
            path = File.Exists(path) ? path : Path.Combine(Server.Get.Files.SharedConfigDirectory, "HubConfiguration.syml");
            SYML = new SYML(path);
            SYML.Load();

            //The Plugin Functions itself
            DummyHandler = new HubDummyHandler();
            EventHandlers = new EventHandlers(this);
            MessageHandler = new MessageHandler(this);
            TeleporterHandler = new TeleporterHandler(this);
            ServerHandler = new ServerHandler(this);
            AddonHandler = new AddonHandler(this);

            base.Load();
        }

        public override void ReloadConfigs()
        {
            SYML.Load();
            
            EventHandlers.Reload();
            MessageHandler.Reload();
            TeleporterHandler.Reload();
            ServerHandler.Reload();
            AddonHandler.ReloadAddons();
        }
    }
}
