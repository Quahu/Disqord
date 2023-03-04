using System.Collections.Generic;

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
        public List<LocalApplicationCommand> UnchangedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.CreatedCommands"/>
        public List<LocalApplicationCommand> CreatedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.ModifiedCommands"/>
        public Dictionary<Snowflake, LocalApplicationCommand> ModifiedCommands { get; }

        /// <inheritdoc cref="IApplicationCommandCacheChanges.DeletedCommandIds"/>
        public List<Snowflake> DeletedCommandIds { get; }

        IReadOnlyList<LocalApplicationCommand> IApplicationCommandCacheChanges.UnchangedCommands => UnchangedCommands;

        IReadOnlyList<LocalApplicationCommand> IApplicationCommandCacheChanges.CreatedCommands => CreatedCommands;

        IReadOnlyDictionary<Snowflake, LocalApplicationCommand> IApplicationCommandCacheChanges.ModifiedCommands => ModifiedCommands;

        IReadOnlyList<Snowflake> IApplicationCommandCacheChanges.DeletedCommandIds => DeletedCommandIds;

        public Changes(bool areInitial)
        {
            AreInitial = areInitial;
            UnchangedCommands = new List<LocalApplicationCommand>();
            CreatedCommands = new List<LocalApplicationCommand>();
            ModifiedCommands = new Dictionary<Snowflake, LocalApplicationCommand>();
            DeletedCommandIds = new List<Snowflake>();
        }
    }
}
