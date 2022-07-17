using Disqord.Models;

namespace Disqord;

public class TransientContextMenuCommand : TransientApplicationCommand, IContextMenuCommand
{
    public TransientContextMenuCommand(IClient client, ApplicationCommandJsonModel model)
        : base(client, model)
    { }
}