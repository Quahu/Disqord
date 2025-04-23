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
                var host = Host.CreateApplicationBuilder(args);

                host.Services.AddSerilog(CreateSerilogLogger(), dispose: true);

                host.Services.AddVoiceExtension();

                host.ConfigureDiscordBot(new DiscordBotHostingContext
                {
                    // The token is set using the DISQORD_TOKEN environment variable.
                    Token = host.Configuration["DISQORD_TOKEN"],

                    // We use slash commands; we don't need any privileged intents.
                    Intents = GatewayIntents.LibraryRecommended & GatewayIntents.Unprivileged,

                    // We don't use text commands, so we disable the default mention prefix.
                    UseMentionPrefix = false
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
                .CreateLogger();
        }
    }
}
