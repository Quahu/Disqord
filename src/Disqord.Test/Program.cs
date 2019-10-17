using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Rest;

namespace Disqord.Test
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            var token = Environment.GetEnvironmentVariable("NOT_QUAHU", EnvironmentVariableTarget.User);
            using (var bot = new DiscordBot(TokenType.Bot, token, new DiscordBotConfiguration
            {
                Prefixes = new[] { "~~" }
            }))
            {
                bot.Logger.MessageLogged += this.Logger_MessageLogged;
                bot.AddModules(Assembly.GetExecutingAssembly());

                var guild = await bot.GetGuildAsync(416256456505950215);
                var channels = await guild.GetChannelsAsync();
                var channel = channels.OfType<RestTextChannel>().FirstOrDefault(x => x.Name == "general");

                bot.Run();
            }
        }

        private void Logger_MessageLogged(object sender, Logging.MessageLoggedEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
