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
                var host = Host.CreateApplicationBuilder(args);

                host.Services.AddSerilog(CreateSerilogLogger(), dispose: true);

                host.Services.Replace(ServiceDescriptor.Singleton<IJsonSerializer, SystemJsonSerializer>());

                host.Services.Configure<DefaultGatewayConfiguration>(x => x.LogsPayloads = true);

                host.ConfigureDiscordBot<TestBot>(new DiscordBotHostingContext
                {
                    Token = host.Configuration["DISQORD_TOKEN"],
                    UseMentionPrefix = false,
                    Prefixes = ["??"],
                    Intents = GatewayIntents.LibraryRecommended | GatewayIntents.DirectMessages | GatewayIntents.DirectReactions
                });

                host.Build().Run();
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
