using SLHub.Interactable;
using Synapse.Api;
using UnityEngine;

namespace SLHub.MessageDummy
{
    public class MessageDummy : IHubDummy , IEnterable
    {
        public MessageDummy(SerializedMessageDummy messageDummy)
        {
            Window = messageDummy.WindowMessage;
            Broadcast = messageDummy.BroadcastMessage;
            Hint = messageDummy.HintMessage;
            Position = messageDummy.Position.Parse().Position;
            LookingAt = messageDummy.LookingAt.Parse().Position;
            Role = messageDummy.Role;
            DisplayName = messageDummy.Name;
            HubDummyHandler.SpawnHubDummy(this);
        }

        public string Window { get; set; }
        public string Broadcast { get; set; }
        public string Hint { get; set; }
        public GameObject GameObject { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookingAt { get; set; }
        public RoleType Role { get; set; }
        public string DisplayName { get; set; }

        public void OnEnter(Player player)
        {
            if (!string.IsNullOrWhiteSpace(Window))
                player.OpenReportWindow(Window.Replace("\\n", "\n"));

            if (!string.IsNullOrWhiteSpace(Broadcast))
                player.SendBroadcast(5, Broadcast.Replace("\\n", "\n"), true);

            if (!string.IsNullOrWhiteSpace(Hint))
                player.GiveTextHint(Hint.Replace("\\n", "\n"));
        }
    }
}
