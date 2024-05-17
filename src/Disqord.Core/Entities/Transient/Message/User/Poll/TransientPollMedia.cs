using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientPollMedia(PollMediaJsonModel model) : TransientEntity<PollMediaJsonModel>(model), IPollMedia
{
    public string? Text => Model.Text.GetValueOrDefault();

    public IEmoji? Emoji => _emoji ??= Optional.ConvertOrDefault(Model.Emoji, TransientEmoji.Create);

    private IEmoji? _emoji;
}
