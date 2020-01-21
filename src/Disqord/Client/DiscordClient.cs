using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        public DiscordClient(TokenType tokenType, string token, DiscordClientConfiguration configuration = null)
            : this(new RestDiscordClient(tokenType, token, configuration?.Logger, configuration?.Serializer), configuration)
        { }

        public DiscordClient(RestDiscordClient restClient, DiscordClientConfiguration configuration = null)
            : base(restClient, configuration?.MessageCache, configuration?.Logger, configuration?.Serializer)
        {
            configuration = configuration ?? DiscordClientConfiguration.Default;

            var shards = configuration.ShardId != null && configuration.ShardCount != null
                ? ((int, int)?) (configuration.ShardId, configuration.ShardCount)
                : null;
            _gateway = new DiscordClientGateway(State, shards);
            _gateway.SetStatus(configuration.Status);
            _gateway.SetActivity(configuration.Activity);
            _getGateway = (client, _) => (client as DiscordClient)._gateway;
        }

        // TODO
        public override ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return default;

            IsDisposed = true;
            _gateway.Dispose();
            return base.DisposeAsync();
        }
    }
}
