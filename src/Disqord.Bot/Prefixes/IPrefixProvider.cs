using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Bot.Prefixes
{
    public interface IPrefixProvider
    {
        ValueTask<IEnumerable<IPrefix>> GetPrefixesAsync(CachedUserMessage message);
    }
}
