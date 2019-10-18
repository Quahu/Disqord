using Disqord.Models;

namespace Disqord
{
    internal static partial class ModelExtensions
    {
        public static FriendSource ToFriendSource(this FriendSourceFlagsModel model)
        {
            if (model.All.HasValue)
                return FriendSource.All;

            if (model.MutualGuilds.HasValue)
                return FriendSource.MutualGuilds;

            if (model.MutualFriends.HasValue)
                return FriendSource.MutualFriends;

            return FriendSource.None;
        }

        public static FriendSourceFlagsModel ToModel(this FriendSource friendSource)
        {
            var model = new FriendSourceFlagsModel();
            if (friendSource == FriendSource.All)
                model.All = true;

            else if (friendSource == FriendSource.MutualGuilds)
                model.MutualGuilds = true;

            else if (friendSource == FriendSource.MutualFriends)
                model.MutualFriends = true;

            return model;
        }
    }
}
