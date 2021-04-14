<div align="center">
    <h1> Disqord </h1>
    <p> An asynchronous Discord API wrapper for .NET 5 that aims to make Discord bot development simple and enjoyable. </p>
    <p> Designed around Microsoft's <a href="https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection">dependency injection</a> abstractions, it integrates seamlessly with the <a href="https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host">Generic Host</a>. </p>
<br>

[![AppVeyor](https://img.shields.io/appveyor/ci/Quahu/disqord.svg?style=flat-square&label=AppVeyor&logo=appveyor)](https://ci.appveyor.com/project/Quahu/disqord)
[![NuGet](https://img.shields.io/nuget/v/Disqord.svg?style=flat-square&label=NuGet&logo=nuget)](https://www.nuget.org/packages/Disqord/)
[![MyGet](https://img.shields.io/myget/disqord/vpre/Disqord.svg?style=flat-square&label=MyGet&logo=nuget)](https://www.myget.org/feed/disqord/package/nuget/Disqord)
[![Discord](https://img.shields.io/discord/416256456505950215.svg?style=flat-square&label=Discord&logo=discord&color=738ADB)](https://discord.gg/eUMSXGZ)
</div>

## Installation
Nightly Disqord builds can pulled as NuGet packages from the MyGet feed: `https://www.myget.org/F/disqord/api/v3/index.json`.

## Provisional Example
Typing `?ping` or `@YourBot ping` in a channel will make the bot respond with `Pong!`.
```cs
using System;
using Disqord.Bot;
using Disqord.Bot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Example
{
    public sealed class Program : DiscordModuleBase
    {
        [Command("ping")]
        public DiscordCommandResult Ping()
            => Response("Pong!");

        private static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    // We will use the environment variable DISQORD_TOKEN for the bot token.
                    x.AddEnvironmentVariables("DISQORD_");
                })
                .ConfigureLogging(x =>
                {
                    x.AddSimpleConsole();
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    bot.Token = context.Configuration["TOKEN"];
                    bot.UseMentionPrefix = true;
                    bot.Prefixes = new[] { "?" };
                })
                .Build();

            try
            {
                using (host)
                {
                    host.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
```
