using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRole : ISnowflakeEntity, IMentionable, IDeletable
    {
        Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null);
    }
}
