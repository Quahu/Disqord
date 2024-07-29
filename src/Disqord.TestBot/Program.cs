using System;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Disqord.Gateway.Api.Default;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Disqord.TestBot
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Host.CreateDefaultBuilder(args)
                    .UseSerilog(CreateSerilogLogger(), dispose: true)
                    .ConfigureDiscordBot<TestBot>((context, bot) =>
                    {
                        bot.Token = context.Configuration["DISQORD_TOKEN"];
                        bot.UseMentionPrefix = false;
                        bot.Prefixes = new[] { "??" };
                        bot.Intents |= GatewayIntents.DirectMessages | GatewayIntents.DirectReactions;
                    })
                    .ConfigureServices(services =>
                    {
                        services.Replace(ServiceDescriptor.Singleton<IJsonSerializer, SystemJsonSerializer>());

                        services.Configure<DefaultGatewayConfiguration>(x => x.LogsPayloads = true);
                    })
                    .UseDefaultServiceProvider(provider =>
                    {
                        provider.ValidateScopes = true;
                        provider.ValidateOnBuild = true;
                    })
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static ILogger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Async(sink =>
                    sink.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        theme: AnsiConsoleTheme.Code))
                .WriteTo.Async(sink =>
                    sink.File(
                        path: $"logs/log-{DateTime.Now:HH_mm_ss}.txt",
                        restrictedToMinimumLevel: LogEventLevel.Verbose,
                        fileSizeLimitBytes: null,
                        buffered: true))
                .CreateLogger();
        }
    }
}
