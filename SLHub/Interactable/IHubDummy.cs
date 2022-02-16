using UnityEngine;

namespace SLHub.Interactable
{
    public interface IHubDummy
    {
        GameObject GameObject { get; set; }

        Vector3 Position { get; set; }

        Vector3 LookingAt { get; set; }

        RoleType Role { get; set; }

        string DisplayName { get; set; }
    }
}
