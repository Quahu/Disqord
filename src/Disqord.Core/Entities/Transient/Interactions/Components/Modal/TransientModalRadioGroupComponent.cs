using Disqord.Models;

namespace Disqord;

public class TransientModalRadioGroupComponent : TransientModalComponent<ModalRadioGroupComponentJsonModel>, IModalRadioGroupComponent
{
    public string CustomId => Model.CustomId;

    public string? Value => Model.Value.HasValue ? Model.Value.Value : null;

    public TransientModalRadioGroupComponent(ModalRadioGroupComponentJsonModel model) : base(model)
    { }
}
