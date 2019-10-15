using System;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Bot;

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
                await bot.AddReactionAsync(566751794148016148, 633054312611971104, new LocalEmoji("🎉"));
                bot.Run();
            }
        }

        private void Logger_MessageLogged(object sender, Logging.MessageLoggedEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
