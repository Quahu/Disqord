using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildMemberRemoveDispatchHandler : DispatchHandler<GuildMemberRemoveJsonModel, MemberLeftEventArgs>
{
    private GuildMemberAddDispatchHandler? _guildMemberAddHandler;

    // GuildId -> MemberId
    private Dictionary<Snowflake, Snowflake>? _lastMemberIds;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        var guildMemberAddHandler = value[GatewayDispatchNames.GuildMemberAdd] as GuildMemberAddDispatchHandler;
        if (guildMemberAddHandler != null)
        {
            _guildMemberAddHandler = guildMemberAddHandler;
            _lastMemberIds = new();
        }

        base.Bind(value);
    }

    public void OnMemberAdd(Snowflake guildId, Snowflake memberId)
    {
        if (_lastMemberIds != null && _lastMemberIds.TryGetValue(guildId, out var lastMemberId) && lastMemberId == memberId)
            _lastMemberIds.Remove(guildId);
    }

    public override ValueTask<MemberLeftEventArgs?> HandleDispatchAsync(IShard shard, GuildMemberRemoveJsonModel model)
    {
        var guild = Client.GetGuild(model.GuildId);
        if (_lastMemberIds != null)
        {
            if (_lastMemberIds.TryGetValue(model.GuildId, out var lastMemberId) && lastMemberId == model.User.Id)
            {
                // If the event is a duplicate, we don't handle it nor trigger event handlers.
                return default;
            }

            // Decrements the guild member count if the received event is not a duplicate.
            guild?.Update(model);

            // Stores the user ID to check in the next received dispatch.
            _lastMemberIds[model.GuildId] = model.User.Id;

            // Notifies GuildMemberAddHandler that the user left, so it can clear its own duplicate cache.
            _guildMemberAddHandler!.OnMemberRemove(model.GuildId, model.User.Id);
        }

        IUser user;
        if (CacheProvider.TryGetMembers(model.GuildId, out var cache) && cache.TryRemove(model.User.Id, out var cachedMember))
        {
            user = cachedMember;
        }
        else
        {
            user = new TransientUser(Client, model.User);
        }

        var e = new MemberLeftEventArgs(model.GuildId, guild, user);
        return new(e);
    }
}
