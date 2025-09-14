using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

public class TransientModalSelectionComponent(ModalSelectionComponentJsonModel model)
    : TransientModalComponent<ModalSelectionComponentJsonModel>(model), IModalSelectionComponent
{
    public string CustomId => Model.CustomId;

    public IReadOnlyList<string> Values => Model.Values;

    public new SelectionComponentType Type => (SelectionComponentType) Model.Type;
}
