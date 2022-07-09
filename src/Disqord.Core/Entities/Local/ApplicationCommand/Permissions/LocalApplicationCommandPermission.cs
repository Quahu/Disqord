using System;
using Qommon;

namespace Disqord
{
    public class LocalApplicationCommandPermission : ILocalConstruct
    {
        public Optional<Snowflake> TargetId { get; set; }

        public Optional<ApplicationCommandPermissionType> Type { get; set; }

        public Optional<bool> HasPermission { get; set; }

        public LocalApplicationCommandPermission()
        { }

        public LocalApplicationCommandPermission(Snowflake targetId, ApplicationCommandPermissionType type, bool hasPermission)
        {
            TargetId = targetId;
            Type = type;
            HasPermission = hasPermission;
        }

        public virtual LocalApplicationCommandPermission Clone()
            => MemberwiseClone() as LocalApplicationCommandPermission;

        object ICloneable.Clone()
            => Clone();
    }
}
