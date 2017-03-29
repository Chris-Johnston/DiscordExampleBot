using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordExampleBot
{
    public class Program
    {
        // Convert our sync main to an async main.
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandHandler handler;

        public async Task Start()
        {
            
            // Define the DiscordSocketClient with a DiscordSocketConfig
            client = new DiscordSocketClient(new DiscordSocketConfig() { LogLevel = LogSeverity.Info });

            var token = "token here";

            // Login and connect to Discord.
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            var map = new DependencyMap();
            map.Add(client);

            handler = new CommandHandler();
            await handler.Install(map);

            // add logger
            client.Log += Log;

            // Log the invite URL on client ready
            client.Ready += Client_Ready;

            // Block this program until it is closed.
            await Task.Delay(-1);
        }

        // log the OAuth2 Invite URL of the bot on client ready so that user can see it on startup
        private async Task Client_Ready()
        {
            var application = await client.GetApplicationInfoAsync();
            await Log(new LogMessage(LogSeverity.Info, "Program",
                $"Invite URL: <https://discordapp.com/oauth2/authorize?client_id={application.Id}&scope=bot>"));
        }

        // Bare minimum Logging function for both DiscordSocketClient and CommandService
        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
