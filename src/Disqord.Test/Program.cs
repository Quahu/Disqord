using System;
using Disqord.Bot;
using Disqord.Bot.Prefixes;
using Disqord.Bot.Sharding;
using Disqord.Extensions.Interactivity;

namespace Disqord.Test
{
    internal sealed class Program : DiscordBotSharder
    {
        private static void Main()
            => new Program().Run();

        private Program() : base(TokenType.Bot, Environment.GetEnvironmentVariable("NOT_QUAHU", EnvironmentVariableTarget.Machine),
            new DefaultPrefixProvider()
                .AddPrefix("~~")
                .AddMentionPrefix(),
            new DiscordBotSharderConfiguration
            {
                Status = UserStatus.Invisible
            })
        {
            Logger.MessageLogged += MessageLogged;
            AddModules(typeof(Program).Assembly);
            AddExtensionAsync(new InteractivityExtension()).GetAwaiter().GetResult();
        }

        private void MessageLogged(object sender, Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);
    }
}
