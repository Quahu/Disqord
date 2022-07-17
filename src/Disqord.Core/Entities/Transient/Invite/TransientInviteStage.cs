using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientInviteStage : TransientClientEntity<InviteStageInstanceJsonModel>, IInviteStage
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IMember> Speakers
    {
        get
        {
            return _speakers ??= Model.Members.ToReadOnlyDictionary((Client, GuildId), (x, _) => x.User.Value.Id, (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientMember(client, guildId, x) as IMember;
            });
        }
    }
    private IReadOnlyDictionary<Snowflake, IMember>? _speakers;

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
