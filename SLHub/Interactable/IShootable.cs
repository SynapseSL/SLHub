using Synapse.Api;
using UnityEngine;

namespace SLHub.Interactable
{
    public interface IShootable
    {
        GameObject GameObject { get; set; }

        void OnShoot(Player player);
    }
}
