using Disqord.Models;

namespace Disqord;

public class TransientTextDisplayComponent(TextDisplayComponentJsonModel model)
    : TransientBaseComponent<TextDisplayComponentJsonModel>(model), ITextDisplayComponent
{
    public string Content => Model.Content;
}
