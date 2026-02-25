using Disqord.Models;

namespace Disqord;

public class TransientModalTextInputComponent(ModalTextInputComponentJsonModel model)
    : TransientModalComponent<ModalTextInputComponentJsonModel>(model), IModalTextInputComponent
{
    public string CustomId => Model.CustomId;

    public string Value => Model.Value;
}
