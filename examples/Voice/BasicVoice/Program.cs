using System;
using Disqord.Bot.Hosting;
using Disqord.Extensions.Voice;
using Disqord.Gateway;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace BasicVoice
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Host.CreateDefaultBuilder(args)
                    .UseSerilog(CreateSerilogLogger(), dispose: true)
                    .ConfigureServices((context, services) =>
                    {
                        services.AddVoiceExtension();
                    })
                    .ConfigureDiscordBot((context, bot) =>
                    {
                        // The token is set using the DISQORD_TOKEN environment variable.
                        bot.Token = context.Configuration["DISQORD_TOKEN"];

                        // We use slash commands; we don't need any privileged intents.
                        bot.Intents &= GatewayIntents.Unprivileged;

                        // We don't use text commands, so we disable the default mention prefix.
                        bot.UseMentionPrefix = false;
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
                .CreateLogger();
        }
    }
}
