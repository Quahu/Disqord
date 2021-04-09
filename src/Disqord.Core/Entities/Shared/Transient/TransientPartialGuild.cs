using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientPartialGuild : TransientEntity<GuildJsonModel>, IPartialGuild
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public string Name => Model.Name;

        public string IconHash => Model.Icon;

        public bool IsOwner => Model.Owner.Value;

        public GuildPermissions Permissions => Model.Permissions.Value;

        public IReadOnlyList<string> Features => Model.Features.ReadOnly();

        public TransientPartialGuild(IClient client, GuildJsonModel model)
            : base(client, model)
        { }
    }
}
