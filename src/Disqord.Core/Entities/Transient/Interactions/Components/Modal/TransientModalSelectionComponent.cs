using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

public class TransientModalSelectionComponent(IClient client, ModalSelectionComponentJsonModel model)
    : TransientModalComponent<ModalSelectionComponentJsonModel>(client, model), IModalSelectionComponent
{
    public string CustomId => Model.CustomId;

    public IReadOnlyList<string> Values => Model.Values;

}
