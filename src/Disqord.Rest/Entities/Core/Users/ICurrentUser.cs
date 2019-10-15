using System;
using System.Threading.Tasks;

namespace Disqord
{
    public interface ICurrentUser : IUser
    {
        string Locale { get; }

        bool IsVerified { get; }

        string Email { get; }

        bool HasMfaEnabled { get; }

        Task ModifyAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null);
    }
}
