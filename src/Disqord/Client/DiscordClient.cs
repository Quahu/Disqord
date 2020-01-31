using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        public DiscordClient(TokenType tokenType, string token, DiscordClientConfiguration configuration = null)
            : this(new RestDiscordClient(tokenType, token, configuration), configuration)
        { }

        public DiscordClient(RestDiscordClient restClient, DiscordClientConfiguration configuration = null)
            : base(restClient, configuration)
        {
            configuration = configuration ?? DiscordClientConfiguration.Default;

            var shards = configuration.ShardId.HasValue && configuration.ShardCount.HasValue
                ? ((int, int)?) (configuration.ShardId.Value, configuration.ShardCount.Value)
                : null;
            _gateway = new DiscordClientGateway(State, shards);
            _gateway.SetStatus(configuration.Status.GetValueOrDefault(UserStatus.Online));
            _gateway.SetActivity(configuration.Activity.GetValueOrDefault());
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
