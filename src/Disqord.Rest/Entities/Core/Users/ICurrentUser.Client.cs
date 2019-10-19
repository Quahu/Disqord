using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface ICurrentUser : IUser
    {
        Task ModifyAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null);
    }
}
