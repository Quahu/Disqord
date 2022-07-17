using Disqord.Models;

namespace Disqord;

public class TransientMessageContextMenuCommand : TransientContextMenuCommand, IMessageContextMenuCommand
{
    public TransientMessageContextMenuCommand(IClient client, ApplicationCommandJsonModel model)
        : base(client, model)
    { }
}