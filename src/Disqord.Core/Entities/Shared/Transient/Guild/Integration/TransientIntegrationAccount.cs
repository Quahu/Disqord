using Disqord.Models;

namespace Disqord
{
    public class TransientIntegrationAccount : TransientEntity<IntegrationAccountJsonModel>, IIntegrationAccount
    {
        public string Id => Model.Id;

        public string Name => Model.Name;

        public TransientIntegrationAccount(IClient client, IntegrationAccountJsonModel model)
            : base(client, model)
        { }
    }
}
