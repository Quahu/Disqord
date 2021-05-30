using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IApplicationTeam"/>
    public class TransientApplicationTeam : TransientEntity<TeamJsonModel>, IApplicationTeam
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IApplicationTeamMember> Members => _members ??= Model.Members.ToReadOnlyDictionary((Client, Id), (x, _) => x.User.Id, (x, tuple) =>
        {
            var (client, guildId) = tuple;
            return new TransientApplicationTeamMember(client, x) as IApplicationTeamMember;
        });
        private IReadOnlyDictionary<Snowflake, IApplicationTeamMember> _members;

        /// <inheritdoc/>
        public Snowflake OwnerId { get; }

        public TransientApplicationTeam(IClient client, TeamJsonModel model)
            : base(client, model)
        { }
    }
}
