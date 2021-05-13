using Disqord.Models;

namespace Disqord
{
    public class TransientIntegrationAccount : TransientEntity<IntegrationAccountJsonModel>, IIntegrationAccount
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
