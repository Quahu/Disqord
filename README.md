<div align="center">
    <h1> Disqord </h1>
    <p> An asynchronous Discord API wrapper for .NET that aims to make Discord bot development simple and enjoyable without needless boilerplate. </p>
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

[![Workflow](https://img.shields.io/github/actions/workflow/status/Quahu/Disqord/myget.yml?branch=master&style=flat-square&label=Workflow&logo=github)](https://github.com/Quahu/Disqord/actions/workflows/nightly.yml?branch=master)
[![NuGet](https://img.shields.io/nuget/v/Disqord.svg?style=flat-square&label=NuGet&logo=nuget&color=blue)](https://www.nuget.org/packages/Disqord/)
[![MyGet](https://img.shields.io/myget/disqord/vpre/Disqord.svg?style=flat-square&label=MyGet&logo=nuget&color=darkorchid)](https://www.myget.org/feed/disqord/package/nuget/Disqord)
[![Discord](https://img.shields.io/discord/416256456505950215.svg?style=flat-square&label=Discord&logo=discord&color=738ADB)](https://discord.gg/eUMSXGZ)
</div>

## Installation
Stable builds are available on NuGet.  
Nightly Disqord builds can be pulled as NuGet packages from the MyGet feed: `https://www.myget.org/F/disqord/api/v3/index.json`.

## Documentation
The Disqord documentation is available on [GitHub Pages](https://quahu.github.io/Disqord/).

## Examples
Explore examples of the library in the [/examples](https://github.com/Quahu/Disqord/tree/master/examples) folder, all of which are licensed under the MIT license.

## Minimal Example
Typing `?ping` or `@YourBot ping` in a channel will make the bot respond with `Pong!`.
```cs
using Disqord.Bot.Commands.Text;
using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using Qmmands;
using Qmmands.Text;

var host = Host.CreateApplicationBuilder(args);
host.ConfigureDiscordBot(new DiscordBotHostingContext
{
    Token = host.Configuration["DISQORD_TOKEN"], // e.g. could be an environment variable
    Prefixes = ["?"]
});

host.Build().Run();

public class ExampleModule : DiscordTextModuleBase
{
    [TextCommand("ping")]
    public IResult Ping()
    {
        return Response("Pong!");
    }
}
```
