using Synapse.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SLHub.Teleporter
{
    public class TeleporterConfig : IConfigSection
    {
        [Description("A list of Dummies that tp a player to a different Location when he walks into them")]
        public List<SerializedTeleporter> Teleporters { get; set; } = new List<SerializedTeleporter>
        {
            new SerializedTeleporter
            {
                Position = new SerializedMapPoint("HCZ_Room3ar", 0f, 2.15f, 9.3f),
                LookingAt = new SerializedMapPoint("HCZ_Room3ar", 0f, 2.15f, 8.3f),
                TeleportTo = new SerializedMapPoint("HCZ_Room3ar", -1.915619f, 2.158142f, -0.07448594f),
                Name = "Example Teleporter",
                Role = RoleType.Tutorial
            }
        };
    }

    [Serializable]
    public class SerializedTeleporter
    {
        [Description("The Position of the Teleporter")]
        public SerializedMapPoint Position { get; set; }
        [Description("Where the Teleporter is looking at")]
        public SerializedMapPoint LookingAt { get; set; }
        [Description("The Loaction where the player is teleport to")]
        public SerializedMapPoint TeleportTo { get; set; }
        [Description("The Role of the Teleporter")]
        public RoleType Role { get; set; }
        [Description("The Name of the Teleporter")]
        public string Name { get; set; }
    }
}
