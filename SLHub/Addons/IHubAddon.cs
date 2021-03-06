using Synapse.Config;

namespace SLHub.Addons
{
    public interface IHubAddon
    {
        void Load();

        void Reload();

        void NewRound();
    }
}
