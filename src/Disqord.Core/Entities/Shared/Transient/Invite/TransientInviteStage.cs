using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientInviteStage: TransientClientEntity<InviteStageInstanceJsonModel>, IInviteStage
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IMember> Speakers
        {
            get
            {
                if (_speakers == null)
                    _speakers = Model.Members.ToReadOnlyDictionary((Client, GuildId), (x, _) => x.User.Value.Id, (x, tuple) =>
                    {
                        var (client, guildId) = tuple;
                        return new TransientMember(client, guildId, x) as IMember;
                    });

                return _speakers;
            }
        }
        private IReadOnlyDictionary<Snowflake, IMember> _speakers;

        /// <inheritdoc/>
        public int ParticipantCount => Model.ParticipantCount;

        /// <inheritdoc/>
        public int SpeakerCount => Model.SpeakerCount;

        /// <inheritdoc/>
        public string Topic => Model.Topic;

        public TransientInviteStage(IClient client, Snowflake guildId, InviteStageInstanceJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
