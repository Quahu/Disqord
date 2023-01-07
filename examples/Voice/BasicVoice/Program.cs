using System;
using Disqord.Bot.Hosting;
using Disqord.Extensions.Voice;
using Disqord.Gateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace BasicVoice
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (var host = new HostBuilder()
                    .ConfigureHostConfiguration(configuration =>
                    {
                        configuration.AddCommandLine(args);
                    })
                    .ConfigureAppConfiguration(configuration =>
                    {
                        configuration.AddCommandLine(args);
                        configuration.AddEnvironmentVariables("DISQORD_");
                    })
                    .ConfigureLogging(logging =>
                    {
                        var logger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Async(writeTo => writeTo
                                .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code))
                            .CreateLogger();

                        logging.AddSerilog(logger, true);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddVoiceExtension();
                    })
                    .ConfigureDiscordBot((context, bot) =>
                    {
                        // The token is set using the DISQORD_TOKEN environment variable.
                        bot.Token = context.Configuration["TOKEN"];

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
                    .Build())
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
