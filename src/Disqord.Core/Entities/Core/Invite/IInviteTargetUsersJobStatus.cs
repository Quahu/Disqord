using System;

namespace Disqord;

/// <summary>
///     Represents the status of the job that processes the target users of an <see cref="IInvite"/> uploaded from a file.
/// </summary>
public interface IInviteTargetUsersJobStatus : IEntity
{
    /// <summary>
    ///     Gets the status of the job.
    /// </summary>
    InviteTargetUsersJobStatus Status { get; }

    /// <summary>
    ///     Gets the total amount of users to process.
    /// </summary>
    int TotalUserCount { get; }

    /// <summary>
    ///     Gets the amount of users processed so far.
    /// </summary>
    int ProcessedUserCount { get; }

    /// <summary>
    ///     Gets when the job was created.
    /// </summary>
    /// <returns>
    ///     The creation time or <see langword="null"/> when it is not available.
    /// </returns>
    DateTimeOffset? CreatedAt { get; }

    /// <summary>
    ///     Gets when the job was completed.
    /// </summary>
    /// <returns>
    ///     The completion time or <see langword="null"/> when the job has not completed yet.
    /// </returns>
    DateTimeOffset? CompletedAt { get; }

    /// <summary>
    ///     Gets the error message of the job.
    /// </summary>
    /// <returns>
    ///     The error message or <see langword="null"/> when the job has not failed.
    /// </returns>
    string? ErrorMessage { get; }
}
