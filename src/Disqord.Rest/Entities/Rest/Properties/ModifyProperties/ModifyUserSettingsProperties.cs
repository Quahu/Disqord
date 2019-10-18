using System;
using System.Collections.Generic;
using System.Globalization;

namespace Disqord
{
    public sealed class ModifyUserSettingsProperties
    {
        public Optional<int> TimezoneOffset { internal get; set; }

        public Optional<Theme> Theme { internal get; set; }

        public Optional<bool> EnableStreamNotifications { internal get; set; }

        public Optional<UserStatus> Status { internal get; set; }

        public Optional<bool> ShowCurrentGame { internal get; set; }

        public Optional<IEnumerable<Snowflake>> RestrictedGuildIds { internal get; set; }

        public Optional<bool> RenderReactions { internal get; set; }

        public Optional<bool> RenderEmbeds { internal get; set; }

        public Optional<bool> EnableCompactMessages { internal get; set; }

        public Optional<CultureInfo> Locale { internal get; set; }

        public Optional<bool> ShowEmbeds { internal get; set; }

        public Optional<bool> ShowAttachments { internal get; set; }

        // TODO?
        //public Optional<IReadOnlyDictionary<Snowflake, int>> GuildPositions { internal get; set; }

        //public IReadOnlyList<RestGuildFolder> GuildFolders { internal get; set; }

        public Optional<bool> AutomaticallyPlayGifs { internal get; set; }

        public Optional<FriendSource> FriendSource { internal get; set; }

        public Optional<UserContentFilterLevel> ContentFilterLevel { internal get; set; }

        public Optional<bool> EnableTts { internal get; set; }

        public Optional<bool> DisableGamesTab { internal get; set; }

        public Optional<bool> EnableDeveloperMode { internal get; set; }

        public Optional<bool> DetectPlatformAccounts { internal get; set; }

        public Optional<bool> RestrictGuildsByDefault { internal get; set; }

        public Optional<bool> ConvertEmojis { internal get; set; }

        public Optional<bool> AnimateEmojis { internal get; set; }

        public Optional<TimeSpan> AfkTimeout { internal get; set; }

        internal ModifyUserSettingsProperties()
        { }
    }
}
