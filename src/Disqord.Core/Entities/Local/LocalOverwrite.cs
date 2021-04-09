using System;
using Disqord.Entities.Local;
using Disqord.Models;

namespace Disqord
{
    public sealed class LocalOverwrite : ILocalEntity, IOverwrite
    {
        public Snowflake TargetId { get; }

        public OverwritePermissions Permissions { get; }

        public OverwriteTargetType TargetType { get; }

        Snowflake IChannelEntity.ChannelId => throw new NotSupportedException("A local overwrite is not bound to a channel.");

        public LocalOverwrite(Snowflake targetId, OverwriteTargetType targetType, OverwritePermissions permissions)
        {
            TargetId = targetId;
            TargetType = targetType;
            Permissions = permissions;
        }

        public LocalOverwrite(IUser target, OverwritePermissions permissions)
            : this(target?.Id ?? throw new ArgumentNullException(nameof(target)), OverwriteTargetType.Member, permissions)
        { }

        public LocalOverwrite(IRole target, OverwritePermissions permissions)
            : this(target?.Id ?? throw new ArgumentNullException(nameof(target)), OverwriteTargetType.Role, permissions)
        { }

        void IJsonUpdatable<OverwriteJsonModel>.Update(OverwriteJsonModel model)
            => throw new NotSupportedException();
    }
}
