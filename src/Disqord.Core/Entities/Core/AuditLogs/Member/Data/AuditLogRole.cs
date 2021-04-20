using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class AuditLogRole
    {
        public Snowflake Id { get; }

        public string Name { get; }

        public AuditLogRole(RoleJsonModel model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
