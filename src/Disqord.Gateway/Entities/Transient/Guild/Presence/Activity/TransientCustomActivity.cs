using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientCustomActivity : TransientActivity, ICustomActivity
{
    /// <inheritdoc/>
    public string? Text => Model.State.GetValueOrDefault();

    /// <inheritdoc/>
    public IEmoji? Emoji
    {
        get
        {
            if (Model.Emoji.GetValueOrDefault() == null)
                return null;

            return _emoji ??= TransientEmoji.Create(Model.Emoji.Value);
        }
    }
    private IEmoji? _emoji;

    public TransientCustomActivity(IClient client, ActivityJsonModel model)
        : base(client, model)
    { }

    public override string ToString()
    {
        return Emoji != null && Text != null
            ? $"{Emoji} {Text}"
            : Emoji != null
                ? Emoji.ToString()!
                : Text!;
    }
}