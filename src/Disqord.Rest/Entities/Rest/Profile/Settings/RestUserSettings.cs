using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord.Rest
{
    public sealed class RestUserSettings : RestDiscordEntity, IUserSettings
    {
        public int TimezoneOffset { get; private set; }

        public Theme Theme { get; private set; }

        public bool EnableStreamNotifications { get; private set; }

        public UserStatus Status { get; private set; }

        public bool ShowCurrentGame { get; private set; }

        public IReadOnlyList<Snowflake> RestrictedGuildIds { get; private set; }

        public bool RenderReactions { get; private set; }

        public bool RenderEmbeds { get; private set; }

        public bool EnableCompactMessages { get; private set; }

        public CultureInfo Locale { get; private set; }

        public bool ShowEmbeds { get; private set; }

        public bool ShowAttachments { get; private set; }

        public IReadOnlyDictionary<Snowflake, int> GuildPositions { get; private set; }

        public IReadOnlyList<RestGuildFolder> GuildFolders { get; private set; }

        public bool AutomaticallyPlayGifs { get; private set; }

        public FriendSource FriendSource { get; private set; }

        public UserContentFilterLevel ContentFilterLevel { get; private set; }

        public bool EnableTts { get; private set; }

        public bool DisableGamesTab { get; private set; }

        public bool EnableDeveloperMode { get; private set; }

        public bool DetectPlatformAccounts { get; private set; }

        public bool RestrictGuildsByDefault { get; private set; }

        public bool ConvertEmojis { get; private set; }

        public bool AnimateEmojis { get; private set; }

        public TimeSpan AfkTimeout { get; private set; }

        IReadOnlyList<IGuildFolder> IUserSettings.GuildFolders => GuildFolders;

        internal RestUserSettings(RestDiscordClient client, UserSettingsModel model) : base(client)
        {
            Update(model);
        }

        internal void Update(UserSettingsModel model)
        {
            TimezoneOffset = model.TimezoneOffset.Value;
            Theme = model.Theme.Value;
            EnableStreamNotifications = model.StreamNotificationsEnabled.Value;
            Status = model.Status.Value;
            ShowCurrentGame = model.ShowCurrentGame.Value;
            RestrictedGuildIds = model.RestrictedGuilds.Value.Select(x => new Snowflake(x)).ToImmutableArray();
            RenderReactions = model.RenderReactions.Value;
            RenderEmbeds = model.RenderEmbeds.Value;
            EnableCompactMessages = model.MessageDisplayCompact.Value;
            Locale = new CultureInfo(model.Locale.Value);
            ShowEmbeds = model.InlineEmbedMedia.Value;
            ShowAttachments = model.InlineAttachmentMedia.Value;

            var guildPositions = new Dictionary<Snowflake, int>();
            var guildFolders = ImmutableArray.CreateBuilder<RestGuildFolder>();
            var guildPosition = 0;
            for (var i = 0; i < model.GuildFolders.Value.Length; i++)
            {
                var guildFolderModel = model.GuildFolders.Value[i];
                for (var j = 0; j < guildFolderModel.GuildIds.Length; j++)
                    guildPositions.Add(guildFolderModel.GuildIds[j], guildPosition++);

                // Check if this is an actual guild folder and not just ungrouped guilds.
                if (guildFolderModel.Id != null)
                    guildFolders.Add(new RestGuildFolder(this, guildFolderModel));
            }

            GuildPositions = new ReadOnlyDictionary<Snowflake, int>(guildPositions);
            GuildFolders = guildFolders.ToImmutable();
            AutomaticallyPlayGifs = model.GifAutoPlay.Value;
            FriendSource = model.FriendSourceFlags.Value.ToFriendSource();
            ContentFilterLevel = model.ExplicitContentFilter.Value;
            EnableTts = model.EnableTtsCommand.Value;
            DisableGamesTab = model.DisableGamesTab.Value;
            EnableDeveloperMode = model.DeveloperMode.Value;
            DetectPlatformAccounts = model.DetectPlatformAccounts.Value;
            RestrictGuildsByDefault = model.DefaultGuildsRestricted.Value;
            ConvertEmojis = model.ConvertEmoticons.Value;
            AnimateEmojis = model.AnimateEmoji.Value;
            AfkTimeout = TimeSpan.FromSeconds(model.AfkTimeout.Value);
        }

        public async Task ModifyAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyUserSettingsAsync(action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}
