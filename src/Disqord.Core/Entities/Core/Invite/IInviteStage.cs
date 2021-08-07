using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an invite stage.
    /// </summary>
    public interface IInviteStage : IGuildEntity, IJsonUpdatable<InviteStageInstanceJsonModel>
    {
        /// <summary>
        ///     Gets the speakers of this stage keyed by the IDs of the members.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IMember> Speakers { get; }

        /// <summary>
        ///     Gets the participant count of this stage.
        /// </summary>
        int ParticipantCount { get; }

        /// <summary>
        ///     Gets the speaker count of this stage.
        /// </summary>
        int SpeakerCount { get; }

        /// <summary>
        ///     Gets the topic of this stage.
        /// </summary>
        string Topic { get; }
    }
}