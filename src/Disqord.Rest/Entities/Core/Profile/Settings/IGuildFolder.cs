using System.Collections.Generic;

namespace Disqord
{
    public partial interface IGuildFolder : IDeletable
    {
        ulong Id { get; }

        string Name { get; }

        IReadOnlyList<Snowflake> GuildIds { get; }

        Color? Color { get; }

        IUserSettings UserSettings { get; }
    }
}