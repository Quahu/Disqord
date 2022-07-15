using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot.Commands.Application;

public interface IApplicationCommandLocalizer
{
    ValueTask LocalizeAsync(IEnumerable<LocalApplicationCommand> globalCommands,
        IReadOnlyDictionary<Snowflake, IEnumerable<LocalApplicationCommand>> guildCommands, CancellationToken cancellationToken = default);
}
