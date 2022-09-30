using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IThreadChannel"/>
public class TransientThreadChannel : TransientMessageGuildChannel, IThreadChannel
{
    /// <inheritdoc/>
    public override Snowflake? CategoryId => throw new InvalidOperationException($"{nameof(TransientThreadChannel)} does not support {nameof(CategoryId)}.");

    /// <inheritdoc/>
    public override int Position => throw new InvalidOperationException($"{nameof(TransientThreadChannel)} does not support {nameof(Position)}.");

    /// <inheritdoc/>
    public override IReadOnlyList<IOverwrite> Overwrites => throw new InvalidOperationException($"{nameof(TransientThreadChannel)} does not support {nameof(Overwrites)}.");

    /// <inheritdoc/>
    public Snowflake ChannelId => Model.ParentId.Value!.Value;

    /// <inheritdoc/>
    public Snowflake CreatorId => Model.OwnerId.Value;

    /// <inheritdoc/>
    public IThreadMember? CurrentMember
    {
        get
        {
            if (!Model.Member.HasValue)
                return null;

            return _currentMember ??= new TransientThreadMember(Client, Model.Member.Value);
        }
    }
    private TransientThreadMember? _currentMember;

    /// <inheritdoc/>
    public int MessageCount => Model.MessageCount.Value;

    /// <inheritdoc/>
    public int MemberCount => Model.MemberCount.Value;

    public IThreadMetadata Metadata => _metadata ??= new TransientThreadMetadata(Model.ThreadMetadata.Value);

    private TransientThreadMetadata? _metadata;

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> TagIds
    {
        get
        {
            if (!Model.AppliedTags.HasValue)
                return Array.Empty<Snowflake>();

            return Model.AppliedTags.Value;
        }
    }

    public TransientThreadChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}
