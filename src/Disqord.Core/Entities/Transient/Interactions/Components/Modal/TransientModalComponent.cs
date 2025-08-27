using Disqord.Models;
using Qommon;

namespace Disqord;

public static class TransientModalComponent
{
    public static IModalComponent Create(IClient client, ModalBaseComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientModalRowComponent(client, Guard.IsOfType<ModalRowComponentJsonModel>(model)),
            ComponentType.StringSelection => new TransientModalSelectionComponent(client, Guard.IsOfType<ModalSelectionComponentJsonModel>(model)),
            ComponentType.TextInput => new TransientModalTextInputComponent(client, Guard.IsOfType<ModalTextInputComponentJsonModel>(model)),
            ComponentType.Label => new TransientModalLabelComponent(client, Guard.IsOfType<ModalLabelComponentJsonModel>(model)),
            _ => new TransientModalComponent<ModalBaseComponentJsonModel>(client, model)
        };
    }
}
