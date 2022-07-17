using System;
using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IThreadMember"/>
public class TransientThreadMember : TransientClientEntity<ThreadMemberJsonModel>, IThreadMember
{
    /// <inheritdoc/>
    public Snowflake Id => Model.UserId.Value;

    /// <inheritdoc/>
    public Snowflake ThreadId => Model.Id.Value;

    /// <inheritdoc/>
    public DateTimeOffset JoinedAt => Model.JoinTimestamp;

    /// <inheritdoc/>
    public int Flags => Model.Flags;

    public TransientThreadMember(IClient client, ThreadMemberJsonModel model)
        : base(client, model)
    { }
}