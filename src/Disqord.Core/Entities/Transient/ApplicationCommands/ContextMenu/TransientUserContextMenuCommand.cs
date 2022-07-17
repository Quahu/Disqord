using Disqord.Models;

namespace Disqord;

public class TransientUserContextMenuCommand : TransientContextMenuCommand, IUserContextMenuCommand
{
    public TransientUserContextMenuCommand(IClient client, ApplicationCommandJsonModel model)
        : base(client, model)
    { }
}