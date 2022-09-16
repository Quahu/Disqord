using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IForumChannel"/>
public class TransientForumChannel : TransientCategorizableGuildChannel, IForumChannel
{
    /// <inheritdoc/>
    public string Topic => Model.Topic.Value;

    /// <inheritdoc/>
    public bool IsAgeRestricted => Model.Nsfw.Value;

    /// <inheritdoc/>
    public TimeSpan Slowmode => TimeSpan.FromSeconds(Model.RateLimitPerUser.GetValueOrDefault());

    /// <inheritdoc/>
    public TimeSpan DefaultAutomaticArchiveDuration => TimeSpan.FromMinutes(Model.DefaultAutoArchiveDuration.GetValueOrDefault(1440));

    /// <inheritdoc/>
    public Snowflake? LastThreadId => Model.LastMessageId.GetValueOrDefault();

    /// <inheritdoc/>
    public IReadOnlyList<IForumTag> Tags
    {
        get
        {
            if (!Model.AvailableTags.HasValue)
                return ReadOnlyList<IForumTag>.Empty;

            if (_availableTags != null)
                return _availableTags;

            return _availableTags = Model.AvailableTags.Value.ToReadOnlyList(model => new TransientForumTag(model));
        }
    }
    private IReadOnlyList<IForumTag>? _availableTags;

    /// <inheritdoc/>
    public IEmoji? DefaultReactionEmoji
    {
        get
        {
            if (Model.DefaultReactionEmoji.GetValueOrDefault() == null)
                return null;

            if (_emoji != null)
                return _emoji;

            var model = Model.DefaultReactionEmoji.Value;
            if (model.EmojiId != null)
            {
                _emoji = new TransientCustomEmoji(model.EmojiId.Value);
            }
            else
            {
                _emoji = new TransientEmoji(model.EmojiName!);
            }

            return _emoji;
        }
    }
    private IEmoji? _emoji;

    /// <inheritdoc/>
    public TimeSpan DefaultThreadSlowmode => TimeSpan.FromSeconds(Model.DefaultThreadRateLimitPerUser.GetValueOrDefault(0));

    public TransientForumChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}
