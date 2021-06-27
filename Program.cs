using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Threading.Tasks;

namespace GTA_VI__Discord_Prank_
{
    internal class Program
    {
        public static DiscordRpcClient client;

        private static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await RPC();
            });
            Console.ReadLine();
        }

        private static readonly RichPresence presence = new RichPresence()
        {
            Details = "In loading screen",
            State = "Story Mode",
            Assets = new Assets()
            {
                LargeImageKey = "image_large",
                LargeImageText = "Pre-release build (v1.0.276.67.2 DEV)",
                SmallImageKey = "image_small",
                SmallImageText = "EnderIce2 (Rockstar Games)"
            }
        };

        public static async Task RPC()
        {
            try
            {
                client = new DiscordRpcClient("857301851430060063")
                {
                    Logger = new ConsoleLogger() { Level = LogLevel.Warning }
                };
                client.RegisterUriScheme();
                client.OnReady += (sender, e) =>
                {
                    Console.WriteLine("-> Ready from user {0}", e.User.Username);
                };
                client.OnPresenceUpdate += (sender, e) =>
                {
                    Console.WriteLine("-> Update {0}", e.Presence);
                };
                client.OnSubscribe += (sender, e) =>
                {
                };
                client.OnUnsubscribe += (sender, e) =>
                {
                };
                client.OnJoin += (sender, e) =>
                {
                };
                client.OnJoinRequested += (sender, e) =>
                {
                };
                presence.Assets = new Assets()
                {
                    LargeImageKey = "image_large",
                    LargeImageText = "Pre-release build (v0.7.29.67.4 DEV)",
                    SmallImageKey = "image_small",
                    SmallImageText = "EnderIce2 (Social Club)"
                };
                presence.Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow
                };
                client.Initialize();
                presence.Secrets = new Secrets()
                {
                    JoinSecret = "gta://join-request?id=89787128776092"
                };
                presence.Party = new Party()
                {
                    ID = Secrets.CreateFriendlySecret(new Random()),
                    Size = 1,
                    Max = 10
                };
                await Task.Delay(10000);
                presence.Details = "In Game";
                presence.State = "Freeway";
                await Task.Delay(10000);
                _ = GetCities();
                client.SetSubscription(EventType.Join | EventType.JoinRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await Task.Delay(-1);
        }

        private static readonly string[] street = { "Labor Place", "Bait Street", "Caesars Place", "Calafia Road", "Mutiny Road", "Chum Street", "Davis Avenue", "Didion Drive", "Dutch London Street Bridge", "Edwood Way", "Exceptionalists Way", "Fudge Lane" };
        private static readonly string[] city = { "Downtown", "Marina", "Temple", "East Beach", "Las Colinas", "Glen Park" };
        private static bool mission = false;

        public static async Task GetCities()
        {
            while (client != null)
            {
                int r1 = new Random().Next(0, street.Length);
                int r2 = new Random().Next(0, city.Length);
                presence.Details = $"In {city[r2]}, {street[r1]}";
                if (mission)
                    presence.State = "In mission";
                else
                    presence.State = "Freeway";
                client.SetPresence(presence);
                if (new Random().Next(0, 100) > 69)
                    mission = !mission;
                await Task.Delay(60000);
            }
        }
    }
}