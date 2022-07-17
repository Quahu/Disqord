using System.ComponentModel;
using Disqord.Models;

namespace Disqord;

public class TransientInteraction : TransientClientEntity<InteractionJsonModel>, IInteraction
{
    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public long __ReceivedAt { get; }

    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake ApplicationId => Model.ApplicationId;

    /// <inheritdoc/>
    public int Version => Model.Version;

    /// <inheritdoc/>
    public InteractionType Type => Model.Type;

    /// <inheritdoc/>
    public string Token => Model.Token;

    public TransientInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, model)
    {
        __ReceivedAt = __receivedAt;
    }

    public static IInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        if (model.User.HasValue || model.Member.HasValue)
            return TransientUserInteraction.Create(client, __receivedAt, model);

        return new TransientInteraction(client, __receivedAt, model);
    }
}