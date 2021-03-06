using Synapse.Api;
using UnityEngine;
using SLHub.Interactable;

namespace SLHub.Teleporter
{
    public class Teleporter : IHubDummy, IEnterable
    {
        public Teleporter(Vector3 position, Vector3 lookat, Vector3 gotopos, RoleType role, string name)
        {
            GoTo = gotopos;
            Position = position;
            LookingAt = lookat;
            Role = role;
            DisplayName = name;
            HubDummyHandler.SpawnHubDummy(this);
        }

        public Vector3 GoTo { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 LookingAt { get; set; }

        public RoleType Role { get; set; }

        public string DisplayName { get; set; }

        public GameObject GameObject { get; set; }

        public void OnEnter(Player player) => player.Position = GoTo;
    }
}
