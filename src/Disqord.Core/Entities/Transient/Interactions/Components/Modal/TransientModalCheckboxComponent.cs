using Disqord.Models;

namespace Disqord;

public class TransientModalCheckboxComponent : TransientModalComponent<ModalCheckboxComponentJsonModel>, IModalCheckboxComponent
{
    public string CustomId => Model.CustomId;

    public bool Value => Model.Value;

    public TransientModalCheckboxComponent(ModalCheckboxComponentJsonModel model) : base(model)
    { }
}
