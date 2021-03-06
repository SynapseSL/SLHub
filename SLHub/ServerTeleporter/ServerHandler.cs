using Synapse;
using Synapse.Api;
using System.Collections.Generic;
using System.Net.Http;
using MEC;
using System.Linq;

namespace SLHub.ServerTeleporter
{
    public class ServerHandler
    {
        public static Dictionary<ServerTeleporter, Dummy> Servers { get; } = new Dictionary<ServerTeleporter, Dummy>();

        internal ServerHandler(PluginClass plugin)
        {
            Plugin = plugin;
            Reload();

            Server.Get.Events.Round.WaitingForPlayersEvent += Waiting;
            Server.Get.Events.Round.RoundRestartEvent += Restart;
        }

        public ServerConfig Config { get; private set; }

        public PluginClass Plugin { get; }

        public void Reload() => Config = Plugin.SYML.GetOrSetDefault("Servers", new ServerConfig());

        private void Restart() => Servers.Clear();

        private void Waiting()
        {
            foreach (var server in Config.Servers)
                new ServerTeleporter(server.Position.Parse().Position, server.LookingAt.Parse().Position, server.Port, server.Role, server.Name);

            Plugin.EventHandlers.coroutines.Add(Timing.RunCoroutine(UpdatePlayers()));
        }

        private IEnumerator<float> UpdatePlayers()
        {
            var apiType = Config.ApiType;
            if (apiType == Api.None) yield break;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            yield return Timing.WaitForSeconds(2f);

            for (; ; )
            {
                switch (apiType)
                {
                    case Api.Northwood:
                        if (string.IsNullOrWhiteSpace(Config.Id) || string.IsNullOrWhiteSpace(Config.Token)) break;

                        var url = $"https://api.scpslgame.com/serverinfo.php?id={Config.Id}&key={Config.Token}&players=true";
                        var response = client.GetAsync(url).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            Logger.Get.Warn($"[SlHub] ErrorCode from CentralServer: {response.StatusCode}\n{response.Content.ReadAsStringAsync().Result}");
                            break;
                        }

                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<NWResponse>(response.Content.ReadAsStringAsync().Result);

                        foreach(var server in Servers)
                        {
                            var responseserver = result.Servers.FirstOrDefault(x => x.Port == server.Key.Port);
                            if (responseserver == null) continue;
                            server.Value.Player.DisplayInfo = server.Key.DisplayName + "\n" + responseserver.Players;
                        }
                        break;

                    case Api.Custom:
                        if (string.IsNullOrWhiteSpace(Config.CustomUrl)) break;

                        response = client.GetAsync(Config.CustomUrl).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            Logger.Get.Warn($"[SlHub] ErrorCode from CustomUrl Server: {response.StatusCode}\n{response.Content.ReadAsStringAsync().Result}");
                            break;
                        }

                        var customresult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NWResponse.Server>>(response.Content.ReadAsStringAsync().Result);

                        foreach (var server in Servers)
                        {
                            var responseserver = customresult.FirstOrDefault(x => x.Port == server.Key.Port);
                            if (responseserver == null) continue;
                            server.Value.Player.DisplayInfo = server.Key.DisplayName + "\n" + responseserver.Players;
                        }
                        break;

                    case Api.Anomalous:
                        if (string.IsNullOrWhiteSpace(Config.Ip)) break;

                        response = client.GetAsync("http://api.slservers.eu/game").Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            Logger.Get.Warn($"[SlHub] ErrorCode from AnomalousServer: {response.StatusCode}\n{response.Content.ReadAsStringAsync().Result}");
                            break;
                        }

                        var anomalousresult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AnomalousServer>>(response.Content.ReadAsStringAsync().Result);

                        foreach (var server in Servers)
                        {
                            var responseserver = anomalousresult.FirstOrDefault(x => x.Address == $"{Config.Ip}:{server.Key.Port}");
                            if (responseserver == null) continue;
                            server.Value.Player.DisplayInfo = server.Key.DisplayName + "\n" + $"{responseserver.Players}/{responseserver.MaxPlayers}";
                        }
                        break;
                }

                yield return Timing.WaitForSeconds(Config.Cooldown);
            }
        }
    }

    public class NWResponse
    {
        public List<Server> Servers { get; set; }

        public class Server
        {
            public ushort Port { get; set; }
            public string Players { get; set; }
        }
    }

    public class AnomalousServer
    {
        public string Address { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
    }
}
