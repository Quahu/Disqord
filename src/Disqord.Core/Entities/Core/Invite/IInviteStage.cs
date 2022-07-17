using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a stage targeted by an invite.
/// </summary>
public interface IInviteStage : IGuildEntity, IJsonUpdatable<InviteStageInstanceJsonModel>
{
    /// <summary>
    ///     Gets the speakers of the stage.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IMember> Speakers { get; }

    /// <summary>
    ///     Gets the participant count of the stage.
    /// </summary>
    int ParticipantCount { get; }

    /// <summary>
    ///     Gets the speaker count of the stage.
    /// </summary>
    int SpeakerCount { get; }

    /// <summary>
    ///     Gets the topic of the stage.
    /// </summary>
    string Topic { get; }
}