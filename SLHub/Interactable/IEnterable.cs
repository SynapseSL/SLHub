using UnityEngine;
using Synapse.Api;

namespace SLHub.Interactable
{
    public interface IEnterable
    {
        Vector3 Position { get; set; }

        void OnEnter(Player player);
    }
}
