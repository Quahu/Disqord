using Disqord.Models;

namespace Disqord;

public class TransientModalTextDisplayComponent(IClient client, ModalTextDisplayComponentJsonModel model)
    : TransientModalComponent<ModalTextDisplayComponentJsonModel>(client, model), IModalTextDisplayComponent
{ }
