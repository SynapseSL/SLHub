using System.ComponentModel;
using Synapse.Config;
using System;
using System.Collections.Generic;

namespace SLHub.ServerTeleporter
{
    public class ServerConfig : IConfigSection
    {
        [Description("If you want to display the Players on the Server Read the Doucmentation https://github.com/SynapseSL/SLHub/wiki")]
        public Api ApiType { get; set; } = Api.None;

        [Description("This belongs to ApiType don't touch it if you dont want to display the amount of players on a server")]
        public float Cooldown { get; set; } = 30f;

        [Description("This belongs to ApiType don't touch it if you dont want to display the amount of players on a server")]
        public string Id { get; set; } = "";

        [Description("This belongs to ApiType don't touch it if you dont want to display the amount of players on a server")]
        public string Token { get; set; } = "";

        [Description("This belongs to ApiType don't touch it if you dont want to display the amount of players on a server")]
        public string Ip { get; set; } = "";

        [Description("This belongs to ApiType don't touch it if you dont want to display the amount of players on a server")]
        public string CustomUrl { get; set; }

        [Description("Here can you setup Dummies that are sending player to a different Server")]
        public List<SerializedServer> Servers { get; set; } = new List<SerializedServer>
        {
            new SerializedServer
            {
                Position = new SerializedMapPoint("HCZ_Room3ar", -9.6736f, 2.15f, 0f),
                LookingAt = new SerializedMapPoint("HCZ_Room3ar", -8f, 2.15f, 0f),
                Port = 7777,
                Name = "Server#1",
                Role = RoleType.ClassD
            }
        };
    }

    [Serializable]
    public class SerializedServer
    {
        [Description("The Position of the ServerTeleporter")]
        public SerializedMapPoint Position { get; set; }
        [Description("Where the Teleporter is looking at")]
        public SerializedMapPoint LookingAt { get; set; }
        [Description("The Port to which the player is redirected")]
        public ushort Port { get; set; }
        [Description("The Role of the ServerTeleporter")]
        public RoleType Role { get; set; }
        [Description("The Name of the ServerTeleporter")]
        public string Name { get; set; }
    }

    public enum Api
    {
        None,
        Northwood,
        Anomalous,
        Custom,
        //The new Synapse networking System
        //Synapse,
    }
}
