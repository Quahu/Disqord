using Disqord.Models;
using Qommon;

namespace Disqord;

public static class TransientModalComponent
{
    public static IModalComponent Create(ModalBaseComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientModalRowComponent(Guard.IsOfType<ModalRowComponentJsonModel>(model)),
            ComponentType.StringSelection or (>= ComponentType.UserSelection and <= ComponentType.ChannelSelection) => new TransientModalSelectionComponent(Guard.IsOfType<ModalSelectionComponentJsonModel>(model)),
            ComponentType.TextInput => new TransientModalTextInputComponent(Guard.IsOfType<ModalTextInputComponentJsonModel>(model)),
            ComponentType.TextDisplay => new TransientModalTextDisplayComponent(Guard.IsOfType<ModalTextDisplayComponentJsonModel>(model)),
            ComponentType.Label => new TransientModalLabelComponent(Guard.IsOfType<ModalLabelComponentJsonModel>(model)),
            _ => new TransientModalComponent<ModalBaseComponentJsonModel>(model)
        };
    }
}
