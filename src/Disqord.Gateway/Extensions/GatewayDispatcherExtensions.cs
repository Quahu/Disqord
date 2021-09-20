using Qommon.Collections.Synchronized;
using Disqord.Models;

namespace Disqord.Gateway
{
    public static class GatewayDispatcherExtensions
    {
        public static IUser GetSharedOrTransientUser(this IGatewayDispatcher dispatcher, UserJsonModel model)
        {
            if (dispatcher.Client.CacheProvider.TryGetUsers(out var cache))
            {
                if (cache.TryGetValue(model.Id, out var user))
                    return user;
            }

            return new TransientUser(dispatcher.Client, model);
        }

        public static CachedSharedUser GetOrAddSharedUser(this IGatewayDispatcher dispatcher, UserJsonModel model)
        {
            if (dispatcher.Client.CacheProvider.TryGetUsers(out var cache))
            {
                return cache.GetOrAdd(model.Id, static(_, tuple) =>
                {
                    var (client, model) = tuple;
                    return new CachedSharedUser(client, model);
                }, (dispatcher.Client, model));
            }

            return null;
        }

        public static CachedMember GetOrAddMember(this IGatewayDispatcher dispatcher, Snowflake guildId, MemberJsonModel model)
        {
            var sharedUser = dispatcher.GetOrAddSharedUser(model.User.Value);
            if (sharedUser == null)
                return null;

            if (dispatcher.Client.CacheProvider.TryGetMembers(guildId, out var cache))
            {
                if (cache.TryGetValue(model.User.Value.Id, out var member))
                {
                    member.Update(model);
                    return member;
                }

                member = new CachedMember(sharedUser, guildId, model);
                cache.Add(model.User.Value.Id, member);
                return member;
            }

            return null;
        }

        public static CachedSharedUser GetOrAddSharedUser(this IGatewayDispatcher dispatcher,
            ISynchronizedDictionary<Snowflake, CachedSharedUser> userCache, UserJsonModel model)
        {
            return userCache.GetOrAdd(model.Id, static(_, tuple) =>
            {
                var (client, model) = tuple;
                return new CachedSharedUser(client, model);
            }, (dispatcher.Client, model));
        }

        public static CachedMember GetOrAddMember(this IGatewayDispatcher dispatcher,
            ISynchronizedDictionary<Snowflake, CachedSharedUser> userCache, ISynchronizedDictionary<Snowflake, CachedMember> memberCache,
            Snowflake guildId, MemberJsonModel model)
        {
            var sharedUser = dispatcher.GetOrAddSharedUser(userCache, model.User.Value);
            if (sharedUser == null)
                return null;

            if (memberCache.TryGetValue(model.User.Value.Id, out var member))
            {
                member.Update(model);
                return member;
            }

            member = new CachedMember(sharedUser, guildId, model);
            memberCache.Add(model.User.Value.Id, member);
            return member;
        }
    }
}
