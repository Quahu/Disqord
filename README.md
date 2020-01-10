# Disqord
[![Build Status](https://img.shields.io/appveyor/ci/Quahu/disqord.svg?style=flat-square)](https://ci.appveyor.com/project/Quahu/disqord)
[![NuGet](https://img.shields.io/nuget/v/Disqord.svg?style=flat-square)](https://www.nuget.org/packages/Disqord/)
[![MyGet](https://img.shields.io/myget/quahu/vpre/Disqord.svg?style=flat-square&label=myget)](https://www.myget.org/feed/quahu/package/nuget/Disqord)
[![The Lab](https://img.shields.io/discord/416256456505950215.svg?style=flat-square&label=discord)](https://discord.gg/eUMSXGZ)  

An asynchronous .NET Core 3.0 Discord API wrapper. 

Inspired by [Discord.Net](https://github.com/RogueException/Discord.Net), [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus), and [discord.py](https://github.com/Rapptz/discord.py).


## Installing
Stable Disqord builds can be pulled from NuGet.
For nightly builds add `https://www.myget.org/F/quahu/api/v3/index.json` (the nightly feed) to your project's package sources and pull from there instead.


## Documentation
There's currently no official documentation for Disqord except for the bundled XML docstrings. For support you should hop in my Discord guild:

[![The Lab](https://discordapp.com/api/guilds/416256456505950215/embed.png?style=banner2)](https://discord.gg/eUMSXGZ)

### A Simple Ping-Pong Bot Example
Typing `?ping` or `@YourBot ping` in the chat would reply with `Pong!`.

```cs
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Prefixes;
using Qmmands;

namespace Example
{
    public sealed class Program
    {
        public static void Main()
        {
            var prefixProvider = new DefaultPrefixProvider()
                .AddPrefix('?')
                .AddMentionPrefix();
            using (var bot = new DiscordBot(TokenType.Bot, "token", prefixProvider))
            {
                bot.AddModule<Commands>();
                bot.Run();
            }
        }
    }

    public sealed class Commands : DiscordModuleBase
    {
        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("Pong!");
    }
}
```
