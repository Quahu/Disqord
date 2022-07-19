using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Gateway;
using Disqord.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qommon.Binding;

namespace Disqord.Bot.Hosting;

/// <summary>
///     Represents the singleton service responsible for handling events between <see cref="DiscordBotBase"/>
///     and the following overridable <see cref="DiscordBotService"/> methods:
///     <list type="bullet">
///         <item>
///             <term> <see cref="DiscordBotService.OnMessageReceived(BotMessageReceivedEventArgs)"/> </term>
///         </item>
///         <item>
///             <term> <see cref="DiscordBotService.OnNonCommandReceived(BotMessageReceivedEventArgs)"/> </term>
///         </item>
///         <item>
///             <term> <see cref="DiscordBotService.OnCommandNotFound(IDiscordCommandContext)"/> </term>
///         </item>
///     </list>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class DiscordBotMasterService : IBindable<DiscordBotBase>
{
    public ILogger<DiscordBotMasterService> Logger { get; }

    public DiscordBotBase Bot => _binder.Value;

    public DiscordBotService[] MessageReceivedServices { get; private set; } = null!;
    public DiscordBotService[] NonCommandReceivedServices { get; private set; } = null!;
    public DiscordBotService[] CommandNotFoundServices { get; private set; } = null!;

    private readonly Binder<DiscordBotBase> _binder;

    public DiscordBotMasterService(
        ILogger<DiscordBotMasterService> logger)
    {
        Logger = logger;

        _binder = new Binder<DiscordBotBase>(this);
    }

    public void Bind(DiscordBotBase value)
    {
        _binder.Bind(value);

        var messageReceivedServices = new List<DiscordBotService>();
        var nonCommandReceivedServices = new List<DiscordBotService>();
        var commandNotFoundServices = new List<DiscordBotService>();
        foreach (var service in Bot.Services.GetServices<DiscordClientService>())
        {
            if (service is not DiscordBotService botService)
                continue;

            if (DiscordClientMasterService.IsOverridden(botService, nameof(DiscordBotService.OnMessageReceived), typeof(BotMessageReceivedEventArgs)))
                messageReceivedServices.Add(botService);

            if (DiscordClientMasterService.IsOverridden(botService, nameof(DiscordBotService.OnNonCommandReceived), typeof(BotMessageReceivedEventArgs)))
                nonCommandReceivedServices.Add(botService);

            if (DiscordClientMasterService.IsOverridden(botService, nameof(DiscordBotService.OnCommandNotFound), typeof(IDiscordCommandContext)))
                commandNotFoundServices.Add(botService);
        }

        // Sorts the services by their priority in a descending order.
        messageReceivedServices.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        nonCommandReceivedServices.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        commandNotFoundServices.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        MessageReceivedServices = messageReceivedServices.ToArray();
        NonCommandReceivedServices = nonCommandReceivedServices.ToArray();
        CommandNotFoundServices = commandNotFoundServices.ToArray();

        Bot.MessageReceived += HandleMessageReceived;
    }

    private async ValueTask HandleMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        BotMessageReceivedEventArgs? args = null;
        if (MessageReceivedServices.Length > 0)
        {
            args = new BotMessageReceivedEventArgs(e);
            foreach (var service in MessageReceivedServices)
            {
                try
                {
                    await service.OnMessageReceived(args).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An exception occurred while executing {0}.{1}().", service.GetType().Name, nameof(DiscordBotService.OnMessageReceived));
                }
            }
        }

        if ((args == null || args.ProcessCommands) && e.Message is IGatewayUserMessage)
        {
            var isCommand = await Bot.ProcessCommandsAsync(e).ConfigureAwait(false);
            if (isCommand)
                return;
        }

        if (NonCommandReceivedServices.Length > 0)
        {
            args ??= new BotMessageReceivedEventArgs(e);
            foreach (var service in NonCommandReceivedServices)
            {
                try
                {
                    await service.OnNonCommandReceived(args).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An exception occurred while executing {0}.{1}().", service.GetType().Name, nameof(DiscordBotService.OnNonCommandReceived));
                }
            }
        }
    }

    internal async ValueTask HandleCommandNotFound(IDiscordCommandContext context)
    {
        foreach (var service in CommandNotFoundServices)
        {
            try
            {
                await service.OnCommandNotFound(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while executing {0}.{1}().", service.GetType().Name, nameof(DiscordBotService.OnCommandNotFound));
            }
        }
    }
}
