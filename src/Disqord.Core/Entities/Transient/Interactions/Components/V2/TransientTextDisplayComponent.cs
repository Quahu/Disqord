using Disqord.Models;

namespace Disqord;

public class TransientTextDisplayComponent(IClient client, TextDisplayComponentJsonModel model)
    : TransientBaseComponent<TextDisplayComponentJsonModel>(client, model), ITextDisplayComponent
{
    public string Content => Model.Content;
}
