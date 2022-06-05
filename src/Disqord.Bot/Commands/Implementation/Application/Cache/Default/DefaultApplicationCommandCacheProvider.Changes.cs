using System.Collections.Generic;
using Qommon.Collections;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandCacheProvider
{
    public class Changes : IApplicationCommandCacheChanges
    {
        /// <inheritdoc />
        public bool Any => CreatedCommands.Count != 0 || ModifiedCommands.Count != 0 || DeletedCommandIds.Count != 0;

        /// <inheritdoc />
        public bool AreInitial { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.UnchangedCommands"/>
        public FastList<LocalApplicationCommand> UnchangedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.CreatedCommands"/>
        public FastList<LocalApplicationCommand> CreatedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.ModifiedCommands"/>
        public Dictionary<Snowflake, LocalApplicationCommand> ModifiedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.DeletedCommandIds"/>
        public FastList<Snowflake> DeletedCommandIds { get; }

        IReadOnlyList<LocalApplicationCommand> IApplicationCommandCacheChanges.UnchangedCommands => UnchangedCommands;

        IReadOnlyList<LocalApplicationCommand> IApplicationCommandCacheChanges.CreatedCommands => CreatedCommands;

        IReadOnlyDictionary<Snowflake, LocalApplicationCommand> IApplicationCommandCacheChanges.ModifiedCommands => ModifiedCommands;

        IReadOnlyList<Snowflake> IApplicationCommandCacheChanges.DeletedCommandIds => DeletedCommandIds;

        public Changes(bool areInitial)
        {
            AreInitial = areInitial;
            UnchangedCommands = new FastList<LocalApplicationCommand>();
            CreatedCommands = new FastList<LocalApplicationCommand>();
            ModifiedCommands = new Dictionary<Snowflake, LocalApplicationCommand>();
            DeletedCommandIds = new FastList<Snowflake>();
        }
    }
}
