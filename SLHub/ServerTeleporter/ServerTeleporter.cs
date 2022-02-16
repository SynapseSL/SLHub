using SLHub.Interactable;
using Synapse.Api;
using UnityEngine;

namespace SLHub.ServerTeleporter
{
    public class ServerTeleporter : IHubDummy, IEnterable
    {
        public ServerTeleporter(Vector3 position, Vector3 lookat, ushort port, RoleType role, string name)
        {
            Port = port;
            Position = position;
            LookingAt = lookat;
            Role = role;
            DisplayName = name;
            ServerHandler.Servers.Add(this, HubDummyHandler.SpawnHubDummy(this));
        }

        public ushort Port { get; set; }
        public GameObject GameObject { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookingAt { get; set; }
        public RoleType Role { get; set; }
        public string DisplayName { get; set; }

        public void OnEnter(Player player) => player.SendToServer(Port);
    }
}
