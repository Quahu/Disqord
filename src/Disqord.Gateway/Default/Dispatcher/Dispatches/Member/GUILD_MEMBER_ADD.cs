using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildMemberAddDispatchHandler : DispatchHandler<GuildMemberAddJsonModel, MemberJoinedEventArgs>
{
    private GuildMemberRemoveDispatchHandler? _guildMemberRemoveHandler;

    // GuildId -> MemberId
    private Dictionary<Snowflake, Snowflake>? _lastMemberIds;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        var guildMemberRemoveHandler = value[GatewayDispatchNames.GuildMemberRemove] as GuildMemberRemoveDispatchHandler;
        if (guildMemberRemoveHandler != null)
        {
            _guildMemberRemoveHandler = guildMemberRemoveHandler;
            _lastMemberIds = new();
        }

        base.Bind(value);
    }

    public void OnMemberRemove(Snowflake guildId, Snowflake memberId)
    {
        if (_lastMemberIds != null && _lastMemberIds.TryGetValue(guildId, out var lastMemberId) && lastMemberId == memberId)
            _lastMemberIds.Remove(guildId);
    }

    public override ValueTask<MemberJoinedEventArgs?> HandleDispatchAsync(IShard shard, GuildMemberAddJsonModel model)
    {
        var guild = Client.GetGuild(model.GuildId);
        if (_lastMemberIds != null)
        {
            if (_lastMemberIds.TryGetValue(model.GuildId, out var lastMemberId) && lastMemberId == model.User.Value.Id)
            {
                // If the event is a duplicate, we don't handle it nor trigger event handlers.
                return default;
            }

            // Increments the guild member count if the received event is not a duplicate.
            guild?.Update(model);

            // Stores the user ID to check in the next received dispatch.
            _lastMemberIds[model.GuildId] = model.User.Value.Id;

            // Notifies GuildMemberRemoveHandler that the user joined, so it can clear its own duplicate cache.
            _guildMemberRemoveHandler!.OnMemberAdd(model.GuildId, model.User.Value.Id);
        }

        var member = Dispatcher.GetOrAddMemberTransient(model.GuildId, model);
        var e = new MemberJoinedEventArgs(guild, member);
        return new(e);
    }
}
