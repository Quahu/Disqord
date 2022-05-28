using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot.Commands.Application;

public interface IApplicationCommandCacheProvider
{
    ValueTask<IApplicationCommandCache> GetCacheAsync(CancellationToken cancellationToken);
}
