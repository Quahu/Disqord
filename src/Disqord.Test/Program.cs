using System;
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
            AddModules(typeof(Program).Assembly);
            AddExtensionAsync(new InteractivityExtension()); // interactivity has no async setup
        }
    }
}
