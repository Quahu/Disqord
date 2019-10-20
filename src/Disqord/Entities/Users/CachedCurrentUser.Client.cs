using System;
using System.Threading.Tasks;

namespace Disqord
{
    public sealed partial class CachedCurrentUser : CachedUser, ICurrentUser
    {
        public Task ModifyAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null)
            => Client.ModifyCurrentUserAsync(action, options);
    }
}