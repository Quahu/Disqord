using System.Threading.Tasks;
using Disqord.Logging;

namespace Disqord.Gateway
{
    public interface IGatewayChunker : ILogging
    {
        Task ChunkAsync(IGatewayGuild guild);
    }
}
