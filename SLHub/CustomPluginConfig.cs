using Synapse.Api.Enum;
using Synapse.Config;
using System.Collections.Generic;
using System.ComponentModel;

namespace SLHub
{
    public class CustomPluginConfig : IConfigSection
    {
        [Description("Where all Players should spawn")]
        public SerializedMapPoint SpawnPoint { get; set; } = new SerializedMapPoint("HCZ_Room3ar", -1.915619f, 2.158142f, -0.07448594f);

        [Description("As which Role the player gets spawned")]
        public RoleType SpawnRole { get; set; } = RoleType.ClassD;

        [Description("If all Ragdolls should be destroyed")]
        public bool DestroyAllRagdolls { get; set; } = true;

        [Description("If players can drop Items")]
        public bool AllowDrop { get; set; } = false;

        [Description("If Players can Escape")]
        public bool AllowEscape { get; set; } = false;

        [Description("If Roles should spawn with they Items")]
        public bool SpawnWithItems { get; set; } = false;

        [Description("If all Doors should be locked")]
        public bool LockAllDoors { get; set; } = false;

        [Description("All Doors that should be locked")]
        public List<DoorType> LockedDoors { get; set; } = new List<DoorType> { DoorType.HCZ_Door };

        [Description("All Doors that should be Open")]
        public List<DoorType> OpenDoors { get; set; } = new List<DoorType> { DoorType.HCZ_Armory };

        [Description("If all Elevators should be locked")]
        public bool LockAllElevators { get; set; } = false;

        [Description("All Elevators that should be locked")]
        public List<ElevatorType> LockedElevators { get; set; } = new List<ElevatorType> { ElevatorType.Scp049 };
    }
}
