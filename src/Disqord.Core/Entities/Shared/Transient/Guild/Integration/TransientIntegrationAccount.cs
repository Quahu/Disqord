using Disqord.Models;

namespace Disqord
{
    public class TransientIntegrationAccount : TransientClientEntity<IntegrationAccountJsonModel>, IIntegrationAccount
    {
        /// <inheritdoc/>
        public string Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        public TransientIntegrationAccount(IClient client, IntegrationAccountJsonModel model)
            : base(client, model)
        { }
    }
}
