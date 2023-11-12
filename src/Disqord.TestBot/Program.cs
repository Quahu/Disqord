using System;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Configuration;
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
                new HostBuilder()
                    .ConfigureHostConfiguration(configuration =>
                    {
                        configuration.AddCommandLine(args);
                    })
                    .ConfigureAppConfiguration(configuration =>
                    {
                        configuration.AddCommandLine(args);
                        configuration.AddEnvironmentVariables("DISQORD_");
                    })
                    .UseSerilog(CreateSerilogLogger(), dispose: true)
                    .ConfigureDiscordBot<TestBot>((context, bot) =>
                    {
                        bot.Token = context.Configuration["TOKEN"];
                        bot.UseMentionPrefix = false;
                        bot.Prefixes = new[] { "??" };
                        bot.Intents |= GatewayIntents.DirectMessages | GatewayIntents.DirectReactions;
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
