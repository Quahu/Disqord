namespace Disqord
{
    public interface IOverwrite : IDeletable
    {
        Snowflake ChannelId { get; }

        Snowflake TargetId { get; }

        OverwritePermissions Permissions { get; }

        OverwriteTargetType TargetType { get; }
    }
}
