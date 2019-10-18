using System;
using System.Collections.Generic;
using System.Globalization;

namespace Disqord
{
    public partial interface IUserSettings : IDiscordEntity
    {
        int TimezoneOffset { get; }

        Theme Theme { get; }

        bool EnableStreamNotifications { get; }

        UserStatus Status { get; }

        bool ShowCurrentGame { get; }

        IReadOnlyList<Snowflake> RestrictedGuildIds { get; }

        bool RenderReactions { get; }

        bool RenderEmbeds { get; }

        bool EnableCompactMessages { get; }

        CultureInfo Locale { get; }

        bool ShowEmbeds { get; }

        bool ShowAttachments { get; }

        IReadOnlyDictionary<Snowflake, int> GuildPositions { get; }

        IReadOnlyList<IGuildFolder> GuildFolders { get; }

        bool AutomaticallyPlayGifs { get; }

        FriendSource FriendSource { get; }

        UserContentFilterLevel ContentFilterLevel { get; }

        bool EnableTts { get; }

        bool DisableGamesTab { get; }

        bool EnableDeveloperMode { get; }

        bool DetectPlatformAccounts { get; }

        bool RestrictGuildsByDefault { get; }

        // TODO
        // object CustomStatus { get; }

        bool ConvertEmojis { get; }

        bool AnimateEmojis { get; }

        TimeSpan AfkTimeout { get; }
    }
}
