<div align="center">
    <h1> Disqord </h1>
    <p> An asynchronous Discord API wrapper for .NET 6 that aims to make Discord bot development simple and enjoyable without needless boilerplate. </p>
    <ul style="list-style-position: inside">
        <li>
            Designed around Microsoft's <a href="https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection">dependency injection</a> abstractions
        </li>
        <li>
            Integrates seamlessly with the <a href="https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host">Generic Host</a>
        </li>
        <li>
            Replaceable components, stateless REST, customizable caching, and more
        </li>
    </ul>
<br>

[![AppVeyor](https://img.shields.io/appveyor/build/Quahu/Disqord/master?style=flat-square&label=AppVeyor&logo=appveyor)](https://ci.appveyor.com/project/Quahu/disqord)
[![NuGet](https://img.shields.io/nuget/v/Disqord.svg?style=flat-square&label=NuGet&logo=nuget&color=blue)](https://www.nuget.org/packages/Disqord/)
[![MyGet](https://img.shields.io/myget/disqord/vpre/Disqord.svg?style=flat-square&label=MyGet&logo=nuget&color=darkorchid)](https://www.myget.org/feed/disqord/package/nuget/Disqord)
[![Discord](https://img.shields.io/discord/416256456505950215.svg?style=flat-square&label=Discord&logo=discord&color=738ADB)](https://discord.gg/eUMSXGZ)
</div>

## Documentation
Documentation is available [here](https://quahu.github.io/Disqord/).

## Installation
Nightly Disqord builds can be pulled as NuGet packages from the MyGet feed: `https://www.myget.org/F/disqord/api/v3/index.json`.

## Minimal Example
Typing `?ping` or `@YourBot ping` in a channel will make the bot respond with `Pong!`.
```cs
using Disqord.Bot;
using Disqord.Bot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Qmmands;

await new HostBuilder()
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
        bot.Prefixes = new[] { "?" };
    })
    .RunConsoleAsync();

public class ExampleModule : DiscordModuleBase
{
    [Command("ping")]
    public DiscordCommandResult Ping()
        => Response("Pong!");
}

```
