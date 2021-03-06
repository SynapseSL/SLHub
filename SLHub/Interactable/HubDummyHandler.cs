using Synapse.Api;
using UnityEngine;
using System.Collections.Generic;
using Synapse;

namespace SLHub.Interactable
{
    public class HubDummyHandler
    {
        public static Dummy SpawnHubDummy<THubDummy>(THubDummy hubDummy) where THubDummy : IHubDummy
        {
            if (PluginClass.Singleton.DummyHandler.Dummies.Contains(hubDummy)) return null;

            var dummy = new Dummy(hubDummy.Position, Quaternion.identity, hubDummy.Role);
            dummy.Player.RemoveDisplayInfo(PlayerInfoArea.Nickname);
            dummy.Player.RemoveDisplayInfo(PlayerInfoArea.Role);
            dummy.Player.DisplayInfo = hubDummy.DisplayName.Replace("\\n","\n");
            dummy.RotateToPosition(hubDummy.LookingAt);

            hubDummy.GameObject = dummy.GameObject;
            PluginClass.Singleton.DummyHandler.AddHubDummy(hubDummy);

            return dummy;
        }

        internal HubDummyHandler()
        {
            Server.Get.Events.Player.PlayerSyncDataEvent += SyncData;
            Server.Get.Events.Player.PlayerShootEvent += Shoot;
            Server.Get.Events.Round.WaitingForPlayersEvent += Waiting;
        }

        public List<IHubDummy> Dummies { get; } = new List<IHubDummy>();
        public List<IShootable> Shootables { get; } = new List<IShootable>();
        public List<IEnterable> Enterables { get; } = new List<IEnterable>();

        private void AddHubDummy(IHubDummy hubDummy)
        {
            Dummies.Add(hubDummy);

            if (hubDummy is IShootable s)
                Shootables.Add(s);

            if (hubDummy is IEnterable e)
                Enterables.Add(e);
        }

        #region Events
        private void Waiting()
        {
            Dummies.Clear();
            Shootables.Clear();
            Enterables.Clear();
        }

        private void SyncData(Synapse.Api.Events.SynapseEventArguments.PlayerSyncDataEventArgs ev)
        {
            foreach (var enter in Enterables)
                if (Vector3.Distance(ev.Player.Position, enter.Position) <= 1.5f)
                    enter.OnEnter(ev.Player);
        }

        private void Shoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            var obj = ev.Player.LookingAt;
            foreach (var shoot in Shootables)
                if (shoot.GameObject == obj)
                    shoot.OnShoot(ev.Player);
        }
        #endregion
    }
}
