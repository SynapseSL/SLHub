using MapGeneration;
using SLHub.Teleporter;
using Synapse.Config;
using System.Collections.Generic;
using System.ComponentModel;

namespace PvPArena
{
    public class HubAddonConfig : IConfigSection
    {
        [Description("A List of all Rooms in which Item Usage is allowed")]
        public List<RoomName> PvpRooms { get; set; } = new List<RoomName>
        {
            RoomName.Outside
        };

        [Description("A list of Dummies that gives the players a Item and then tp them")]
        public List<SerializedItemDummy> ItemHolders { get; set; } = new List<SerializedItemDummy>
        {
            new SerializedItemDummy
            {
                Teleporter = new SerializedTeleporter
                {
                    Name = "Medkit",
                    Role = RoleType.Scientist,
                    Position = new SerializedMapPoint("HCZ_Room3ar", 0f, 2.15f, -9.3f),
                    LookingAt = new SerializedMapPoint("HCZ_Room3ar", 0f, 2.15f, -8.3f),
                    TeleportTo = new SerializedMapPoint("HCZ_Room3ar", -1.915619f, 2.158142f, -0.07448594f),
                },
                Item = new SerializedItem
                {
                    ID = (int)ItemType.Medkit,
                    Durabillity = 0,
                    XSize = 1,
                    YSize = 1,
                    ZSize = 1,
                }
            }
        };
    }

    public class SerializedItemDummy
    {
        public SerializedTeleporter Teleporter { get; set; }
        public SerializedItem Item { get; set; }
    }
}
