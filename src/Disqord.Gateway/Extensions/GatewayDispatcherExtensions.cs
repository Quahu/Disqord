using System.ComponentModel;
using Disqord.Models;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class GatewayDispatcherExtensions
{
    public static IUser GetSharedUserTransient(this IGatewayDispatcher dispatcher, UserJsonModel userModel)
    {
        if (dispatcher.Client.CacheProvider.TryGetUsers(out var cache))
        {
            if (cache.TryGetValue(userModel.Id, out var user))
                return user;
        }

        return new TransientUser(dispatcher.Client, userModel);
    }

    public static CachedSharedUser? GetOrAddSharedUser(this IGatewayDispatcher dispatcher, UserJsonModel userModel)
    {
        if (dispatcher.Client.CacheProvider.TryGetUsers(out var cache))
        {
            return cache.GetOrAdd(userModel.Id, static (_, state) =>
            {
                var (client, model) = state;
                return new CachedSharedUser(client, model);
            }, (dispatcher.Client, model: userModel));
        }

        return null;
    }

    public static CachedMember? GetOrAddMember(this IGatewayDispatcher dispatcher, Snowflake guildId, MemberJsonModel memberModel)
    {
        if (!memberModel.User.HasValue)
            return null;

        var sharedUser = dispatcher.GetOrAddSharedUser(memberModel.User.Value);
        if (sharedUser == null)
            return null;

        if (dispatcher.Client.CacheProvider.TryGetMembers(guildId, out var cache))
        {
            if (cache.TryGetValue(memberModel.User.Value.Id, out var member))
            {
                member.Update(memberModel);
                return member;
            }

            member = new CachedMember(sharedUser, guildId, memberModel);
            cache.Add(memberModel.User.Value.Id, member);
            return member;
        }

        return null;
    }

    public static IMember GetOrAddMemberTransient(this IGatewayDispatcher dispatcher, Snowflake guildId, MemberJsonModel memberModel)
    {
        OptionalGuard.HasValue(memberModel.User);

        var member = dispatcher.GetOrAddMember(guildId, memberModel);
        return member ?? new TransientMember(dispatcher.Client, guildId, memberModel) as IMember;
    }

    public static CachedSharedUser GetOrAddSharedUser(this IGatewayDispatcher dispatcher,
        ISynchronizedDictionary<Snowflake, CachedSharedUser> userCache, UserJsonModel model)
    {
        return userCache.GetOrAdd(model.Id, static (_, state) =>
        {
            var (client, model) = state;
            return new CachedSharedUser(client, model);
        }, (dispatcher.Client, model));
    }

    public static CachedMember? GetOrAddMember(this IGatewayDispatcher dispatcher,
        ISynchronizedDictionary<Snowflake, CachedSharedUser> userCache, ISynchronizedDictionary<Snowflake, CachedMember> memberCache,
        Snowflake guildId, MemberJsonModel memberModel)
    {
        if (!memberModel.User.HasValue)
            return null;

        var sharedUser = dispatcher.GetOrAddSharedUser(userCache, memberModel.User.Value);
        if (memberCache.TryGetValue(memberModel.User.Value.Id, out var member))
        {
            member.Update(memberModel);
            return member;
        }

        member = new CachedMember(sharedUser, guildId, memberModel);
        memberCache.Add(memberModel.User.Value.Id, member);
        return member;
    }

    public static IMember GetOrAddMemberTransient(this IGatewayDispatcher dispatcher,
        ISynchronizedDictionary<Snowflake, CachedSharedUser> userCache, ISynchronizedDictionary<Snowflake, CachedMember> memberCache,
        Snowflake guildId, MemberJsonModel memberModel)
    {
        OptionalGuard.HasValue(memberModel.User);

        var member = dispatcher.GetOrAddMember(userCache, memberCache, guildId, memberModel);
        return member ?? new TransientMember(dispatcher.Client, guildId, memberModel) as IMember;
    }
}
