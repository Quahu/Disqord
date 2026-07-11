namespace Disqord;

/// <summary>
///     Represents the status of the job that processes the target users of an <see cref="IInvite"/> uploaded from a file.
/// </summary>
public enum InviteTargetUsersJobStatus
{
    /// <summary>
    ///     The default value.
    /// </summary>
    Unspecified = 0,

    /// <summary>
    ///     The job is still being processed.
    /// </summary>
    Processing = 1,

    /// <summary>
    ///     The job has been completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    ///     The job has failed; see <see cref="IInviteTargetUsersJobStatus.ErrorMessage"/> for more details.
    /// </summary>
    Failed = 3
}
