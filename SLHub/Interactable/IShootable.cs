using UnityEngine;
using Synapse.Api;

namespace SLHub.Interactable
{
    public interface IShootable
    {
        GameObject GameObject { get; set; }

        void OnShoot(Player player);
    }
}
