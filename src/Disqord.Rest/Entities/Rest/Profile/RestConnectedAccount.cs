using Disqord.Models;

namespace Disqord.Rest
{
    public class RestConnectedAccount : RestDiscordEntity
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public bool IsVerified { get; private set; }

        internal RestConnectedAccount(RestDiscordClient client, ConnectedAccountModel model) : base(client)
        {
            Id = model.Id;
            Name = model.Name;
            Type = model.Type;
            IsVerified = model.Verified;
        }

        public override string ToString()
            => $"{Type}: {Name}";
    }
}
