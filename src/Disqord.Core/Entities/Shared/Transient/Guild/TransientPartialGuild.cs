using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientPartialGuild : TransientClientEntity<GuildJsonModel>, IPartialGuild
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public bool IsOwner => Model.Owner.Value;

        /// <inheritdoc/>
        public GuildPermissions Permissions => Model.Permissions.Value;

        /// <inheritdoc/>
        public IReadOnlyList<string> Features => Model.Features.ReadOnly();

        public TransientPartialGuild(IClient client, GuildJsonModel model)
            : base(client, model)
        { }
    }
}
