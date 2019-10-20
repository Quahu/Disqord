using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IRole : ISnowflakeEntity, IMentionable, IDeletable
    {
        Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null);
    }
}
