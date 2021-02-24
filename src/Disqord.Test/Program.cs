using System;
using System.Linq;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Disqord.Test
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(x =>
                {
                    x.AddCommandLine(args);
                })
                .ConfigureAppConfiguration(x =>
                {
                    x.AddCommandLine(args);
                    x.AddEnvironmentVariables("DISQORD_");
                })
                .ConfigureLogging(x =>
                {
                    var logger = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                        .WriteTo.File($"logs/log-{DateTime.Now:HH_mm_ss}.txt", restrictedToMinimumLevel: LogEventLevel.Verbose, fileSizeLimitBytes: null, buffered: true)
                        .CreateLogger();
                    x.AddSerilog(logger, true);

                    x.Services.Remove(x.Services.First(x => x.ServiceType == typeof(ILogger<>)));
                    x.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                })
                .ConfigureServices((context, services) =>
                {

                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    bot.Token = context.Configuration["TOKEN"];
                    bot.Intents = GatewayIntents.All - GatewayIntent.Presences;
                    bot.UseMentionPrefix = true;
                    bot.Prefixes = new[] { "?", "!" };
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
