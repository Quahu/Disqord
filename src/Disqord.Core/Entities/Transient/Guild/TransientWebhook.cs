using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientWebhook : TransientClientEntity<WebhookJsonModel>, IWebhook
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake? ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public Snowflake? GuildId => Model.GuildId.GetValueOrDefault();

    /// <inheritdoc/>
    public string? Name => Model.Name;

    /// <inheritdoc/>
    public string? AvatarHash => Model.Avatar;

    /// <inheritdoc/>
    public IUser? Creator
    {
        get
        {
            if (Model.User.HasValue && _creator == null)
                _creator = new TransientUser(Client, Model.User.Value);

            return _creator;
        }
    }
    private IUser? _creator;

    /// <inheritdoc/>
    public string? Token => Model.Token.GetValueOrDefault();

    /// <inheritdoc/>
    public WebhookType Type => Model.Type;

    public TransientWebhook(IClient client, WebhookJsonModel model)
        : base(client, model)
    { }
}
