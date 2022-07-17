using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Text;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    internal async ValueTask<bool> ProcessCommandsAsync(MessageReceivedEventArgs e)
    {
        // We check if the message is suitable for execution.
        // By default excludes bot messages.
        var message = (e.Message as IGatewayUserMessage)!;
        try
        {
            if (!await OnMessage(message).ConfigureAwait(false))
                return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while executing the check message callback.");
            return false;
        }

        // We get the prefixes from the prefix provider.
        IEnumerable<IPrefix?>? prefixes;
        try
        {
            prefixes = await Prefixes.GetPrefixesAsync(message).ConfigureAwait(false);
            if (prefixes == null)
                return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while getting the prefixes.");
            return false;
        }

        // We try to find a prefix in the message.
        IPrefix? foundPrefix = null;
        ReadOnlyMemory<char> output = default;
        try
        {
            foreach (var prefix in prefixes)
            {
                Guard.IsNotNull(prefix);

                if (prefix.TryFind(message, out output))
                {
                    foundPrefix = prefix;
                    break;
                }
            }

            if (foundPrefix == null)
                return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while finding the prefixes in the message.");
            return false;
        }

        // We create a text command context for Qmmands.
        IDiscordTextCommandContext context;
        try
        {
            context = CreateTextCommandContext(foundPrefix, output, message, e.Channel);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while creating the text command context.");
            return false;
        }

        // We run the common execution logic.
        await ExecuteAsync(context).ConfigureAwait(false);
        return true;
    }
}
