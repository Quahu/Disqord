using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

public class TransientModalCheckboxGroupComponent : TransientModalComponent<ModalCheckboxGroupComponentJsonModel>, IModalCheckboxGroupComponent
{
    public string CustomId => Model.CustomId;

    public IReadOnlyList<string> Values => Model.Values;

    public TransientModalCheckboxGroupComponent(ModalCheckboxGroupComponentJsonModel model) : base(model)
    { }
}
