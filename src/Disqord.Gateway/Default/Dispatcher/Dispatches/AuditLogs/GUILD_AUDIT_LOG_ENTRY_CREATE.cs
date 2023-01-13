using System.Threading.Tasks;
using Disqord.AuditLogs;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildAuditLogEntryCreateDispatchHandler : DispatchHandler<GuildAuditLogEntryCreateJsonModel, AuditLogCreatedEventArgs>
{
    public override ValueTask<AuditLogCreatedEventArgs?> HandleDispatchAsync(IShard shard, GuildAuditLogEntryCreateJsonModel model)
    {
        var auditLog = TransientAuditLog.Create(Client, model.GuildId, null, model);
        var e = new AuditLogCreatedEventArgs(model.GuildId, auditLog);
        return new(e);
    }
}
