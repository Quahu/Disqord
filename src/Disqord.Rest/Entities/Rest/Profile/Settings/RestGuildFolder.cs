using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGuildFolder : RestDiscordEntity, IGuildFolder
    {
        public ulong Id { get; }

        public string Name { get; }

        public IReadOnlyList<Snowflake> GuildIds { get; }

        public Color? Color { get; }

        public RestUserSettings UserSettings { get; }

        IUserSettings IGuildFolder.UserSettings => UserSettings;

        internal RestGuildFolder(RestUserSettings userSettings, GuildFolderModel model) : base(userSettings.Client)
        {
            Id = model.Id.Value;
            Name = model.Name;
            GuildIds = model.GuildIds.ToSnowflakeList();
            Color = model.Color;
            UserSettings = userSettings;
        }

        Task IDeletable.DeleteAsync(RestRequestOptions options)
            => throw new NotImplementedException();
    }
}