using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Voice;
using Qommon;

namespace Disqord.Extensions.Voice
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class VoiceEntityExtensions
    {
        public static ValueTask<IVoiceConnection> ConnectAsync(this IVocalGuildChannel channel, CancellationToken cancellationToken = default)
        {
            var client = Guard.IsAssignableToType<DiscordClientBase>(channel.Client);
            var extension = client.GetRequiredExtension<VoiceExtension>();
            return extension.ConnectAsync(channel.GuildId, channel.Id, cancellationToken);
        }
    }
}
