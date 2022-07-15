using System;
using System.Collections.Generic;

namespace Disqord.Bot.Commands.Application;

public interface IApplicationCommandCache : IAsyncDisposable
{
    IApplicationCommandCacheChanges GetChanges(Snowflake? guildId, IEnumerable<LocalApplicationCommand> commands);

    void ApplyChanges(Snowflake? guildId, IApplicationCommandCacheChanges changes, IEnumerable<IApplicationCommand> commands);
}
