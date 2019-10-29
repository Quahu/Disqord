namespace Disqord
{
    public partial interface IRole : ISnowflakeEntity, IMentionable, IDeletable
    {
        string Name { get; }

        Color? Color { get; }

        bool IsHoisted { get; }

        int Position { get; }

        GuildPermissions Permissions { get; }

        bool IsManaged { get; }

        bool IsMentionable { get; }

        Snowflake GuildId { get; }

        bool IsDefault { get; }
    }
}
