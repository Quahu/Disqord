using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientInviteTargetUsersJobStatus : TransientEntity<InviteTargetUsersJobStatusJsonModel>, IInviteTargetUsersJobStatus
{
    /// <inheritdoc/>
    public InviteTargetUsersJobStatus Status => Model.Status;

    /// <inheritdoc/>
    public int TotalUserCount => Model.TotalUsers;

    /// <inheritdoc/>
    public int ProcessedUserCount => Model.ProcessedUsers;

    /// <inheritdoc/>
    public DateTimeOffset? CreatedAt => Model.CreatedAt;

    /// <inheritdoc/>
    public DateTimeOffset? CompletedAt => Model.CompletedAt;

    /// <inheritdoc/>
    public string? ErrorMessage => Model.ErrorMessage.GetValueOrDefault();

    public TransientInviteTargetUsersJobStatus(InviteTargetUsersJobStatusJsonModel model)
        : base(model)
    { }
}
