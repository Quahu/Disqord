using Disqord.Models;

namespace Disqord;

public class TransientModalTextDisplayComponent(ModalTextDisplayComponentJsonModel model)
    : TransientModalComponent<ModalTextDisplayComponentJsonModel>(model), IModalTextDisplayComponent;
