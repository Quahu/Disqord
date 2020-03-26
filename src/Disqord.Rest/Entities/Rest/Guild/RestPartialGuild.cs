using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestPartialGuild : RestSnowflakeEntity
    {
        public string Name { get; }

        public string IconHash { get; }

        public IReadOnlyList<string> Features { get; }

        public bool IsOwner { get; }

        public GuildPermissions Permissions { get; }

        internal RestPartialGuild(RestDiscordClient client, GuildModel model) : base(client, model.Id)
        {
            Name = model.Name.Value;
            IconHash = model.Icon.Value;
            Features = model.Features.Value.ReadOnly();
            IsOwner = model.Owner.Value;
            Permissions = model.Permissions.Value;
        }

        public string GetIconUrl(ImageFormat format = default, int size = 2048)
            => Discord.Cdn.GetGuildIconUrl(Id, IconHash, format, size);

        public override string ToString()
            => Name;
    }
}
