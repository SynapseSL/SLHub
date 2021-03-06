using SLHub.Interactable;
using SLHub.Teleporter;
using Synapse.Config;
using UnityEngine;
using Synapse.Api;

namespace PvPArena
{
    public class ItemHolder : IHubDummy, IEnterable
    {
        public ItemHolder(SerializedItem item, SerializedTeleporter teleporter)
        {
            Item = item;
            TeleportBack = teleporter.TeleportTo.Parse().Position;
            Position = teleporter.Position.Parse().Position;
            LookingAt = teleporter.LookingAt.Parse().Position;
            Role = teleporter.Role;
            DisplayName = teleporter.Name;
            var dummy = HubDummyHandler.SpawnHubDummy(this);
            dummy.HeldItem = SynapseController.Server.ItemManager.GetBaseType(item.ID);
        }

        public SerializedItem Item { get; set; }
        public Vector3 TeleportBack { get; set; }
        public GameObject GameObject { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookingAt { get; set; }
        public RoleType Role { get; set; }
        public string DisplayName { get; set; }

        public void OnEnter(Player player)
        {
            player.Position = TeleportBack;
            player.Inventory.AddItem(Item.Parse());
        }
    }
}
