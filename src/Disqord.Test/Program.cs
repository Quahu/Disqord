using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Disqord.Bot.Hosting;
using Disqord.Gateway.Api;
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
            try
            {
                using (var host = new HostBuilder()
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
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                            .WriteTo.File($"logs/log-{DateTime.Now:HH_mm_ss}.txt", restrictedToMinimumLevel: LogEventLevel.Verbose, fileSizeLimitBytes: null, buffered: true)
                            .CreateLogger();

                        x.AddSerilog(logger, true);

                        x.Services.Remove(x.Services.First(x => x.ServiceType == typeof(ILogger<>)));
                        x.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    })
                    .ConfigureDiscordBot((context, bot) =>
                    {
                        bot.Token = context.Configuration["TOKEN"];
                        bot.UseMentionPrefix = true;
                        bot.Prefixes = new[] { "?", "!" };
                        bot.Intents |= GatewayIntent.DirectMessages | GatewayIntent.DirectReactions;
                    })
                    .UseDefaultServiceProvider(x =>
                    {
                        x.ValidateScopes = true;
                        x.ValidateOnBuild = true;
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
