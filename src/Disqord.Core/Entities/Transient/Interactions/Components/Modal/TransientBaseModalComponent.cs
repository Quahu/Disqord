using Disqord.Models;

namespace Disqord;

public class TransientModalComponent<TModalComponentModel>(IClient client, ModalBaseComponentJsonModel model)
    : TransientBaseComponent<TModalComponentModel>(client, model), IModalComponent
    where TModalComponentModel : ModalBaseComponentJsonModel
{
    void IJsonUpdatable<ModalBaseComponentJsonModel>.Update(ModalBaseComponentJsonModel model)
    {
        ((IJsonUpdatable<ModalBaseComponentJsonModel>) this).Update(model);
    }
}
