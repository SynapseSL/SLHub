using Synapse.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SLHub.MessageDummy
{
    public class MessageConfig : IConfigSection
    {
        public List<SerializedMessageDummy> MessageDummies { get; set; } = new List<SerializedMessageDummy>
        {
            new SerializedMessageDummy
            {
                BroadcastMessage = "",
                HintMessage = "",
                Name = "Window",
                Role = RoleType.Tutorial,
                WindowMessage = "TestMessage",
                Position = new SerializedMapPoint("HCZ_Room3ar", -3f, 3f, -2.4f),
                LookingAt = new SerializedMapPoint("HCZ_Room3ar", -4f, 3f, -3.4f),
            }
        };
    }

    [Serializable]
    public class SerializedMessageDummy
    {
        [Description("The Position of the MessageDummy")]
        public SerializedMapPoint Position { get; set; }
        [Description("Where the MessageDummy is looking at")]
        public SerializedMapPoint LookingAt { get; set; }
        [Description("The Role of the MessageDummy")]
        public RoleType Role { get; set; }
        [Description("The Name of the MessageDummy")]
        public string Name { get; set; }
        [Description("The Message of the Window that appears")]
        public string WindowMessage { get; set; }
        [Description("The Message of the Hint that appears")]
        public string HintMessage { get; set; }
        [Description("The Message of the broadcast that appears")]
        public string BroadcastMessage { get; set; }
    }
}
