using Disqord.Models;

namespace Disqord;

public class TransientModalTextInputComponent(IClient client, ModalTextInputComponentJsonModel model)
    : TransientModalComponent<ModalTextInputComponentJsonModel>(client, model), IModalTextInputComponent
{
    public string CustomId => Model.CustomId;

    public string Value => Model.Value;
}
