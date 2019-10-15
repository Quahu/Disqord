using System;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IRole : ISnowflakeEntity, IMentionable, IDeletable
    {
        string Name { get; }

        Color Color { get; }

        bool IsHoisted { get; }

        int Position { get; }

        GuildPermissions Permissions { get; }

        bool IsManaged { get; }

        bool IsMentionable { get; }

        Snowflake GuildId { get; }

        Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null);
    }
}
