using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientInvite : TransientClientEntity<InviteJsonModel>, IInvite
{
    /// <inheritdoc/>
    public Snowflake? ChannelId => Model.Channel?.Id;

    /// <inheritdoc/>
    public string Code => Model.Code;

    /// <inheritdoc/>
    public IInviteChannel? Channel
    {
        get
        {
            if (_channel == null && Model.Channel != null)
                _channel = new TransientInviteChannel(Client, Model.Channel);

            return _channel;
        }
    }
    private IInviteChannel? _channel;

    /// <inheritdoc/>
    public IUser? Inviter
    {
        get
        {
            if (!Model.Inviter.HasValue)
                return null;

            return _inviter ??= new TransientUser(Client, Model.Inviter.Value);
        }
    }
    private IUser? _inviter;

    /// <inheritdoc/>
    public int? ApproximateMemberCount => Model.ApproximateMemberCount.GetValueOrNullable();

    /// <inheritdoc/>
    public DateTimeOffset? ExpiresAt => Model.ExpiresAt.Value;

    public TransientInvite(IClient client, InviteJsonModel model)
        : base(client, model)
    { }

    public static IInvite Create(IClient client, InviteJsonModel model)
    {
        return model.Guild.HasValue
            ? new TransientGuildInvite(client, model)
            : new TransientInvite(client, model);
    }
}
