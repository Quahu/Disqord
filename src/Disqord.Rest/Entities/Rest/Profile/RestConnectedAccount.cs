using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestConnectedAccount : RestDiscordEntity
    {
        public bool IsVerified { get; private set; }

        public string Type { get; private set; }

        public string Id { get; private set; }

        public string Name { get; private set; }

        internal RestConnectedAccount(RestDiscordClient client, ConnectedAccountModel model) : base(client)
        {
            Update(model);
        }

        internal void Update(ConnectedAccountModel model)
        {
            IsVerified = model.Verified;
            Type = model.Type;
            Id = model.Id;
            Name = model.Name;
        }

        public override string ToString()
            => $"{Type}: {Name}";
    }
}
