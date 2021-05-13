using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientIntegrationApplication : TransientEntity<IntegrationApplicationJsonModel>, IIntegrationApplication
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public string Name => Model.Name;

        public string IconHash => Model.Icon;

        public string Description => Model.Description;

        public string Summary => Model.Summary;

        public IUser Bot => _bot ??= Optional.ConvertOrDefault(Model.Bot, (model, client) => new TransientUser(client, model), Client);
        private TransientUser _bot;

        public TransientIntegrationApplication(IClient client, IntegrationApplicationJsonModel model)
            : base(client, model)
        { }
    }
}
