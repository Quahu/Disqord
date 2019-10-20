using System.Collections.Concurrent;
using System.Linq;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    public abstract partial class CachedUser : CachedSnowflakeEntity, IUser
    {
        public virtual string Name { get; protected set; }

        public virtual string Discriminator { get; protected set; }

        public virtual string AvatarHash { get; protected set; }

        public virtual bool IsBot { get; }

        public string Tag => $"{Name}#{Discriminator}";

        public virtual string Mention => Discord.MentionUser(this);

        public virtual CachedRelationship Relationship
        {
            get
            {
                var currentUser = Client.CurrentUser;
                return currentUser != null && currentUser.Relationships.TryGetValue(Id, out var relationship)
                    ? relationship
                    : null;
            }
        }

        public string Note
        {
            get
            {
                var currentUser = Client.CurrentUser;
                return currentUser != null && currentUser.Notes.TryGetValue(Id, out var note)
                    ? note
                    : null;
            }
        }

        public virtual CachedDmChannel DmChannel => Client.DmChannels.Values.FirstOrDefault(x => x.Recipient.Id == Id);

        public abstract UserStatus Status { get; }

        public abstract Activity Activity { get; }

        public UserStatus DesktopStatus => ClientStatus.TryGetValue("desktop", out var desktopStatus)
                ? desktopStatus
                : UserStatus.Offline;

        public UserStatus MobileStatus => ClientStatus.TryGetValue("mobile", out var mobileStatus)
                ? mobileStatus
                : UserStatus.Offline;

        public UserStatus WebStatus => ClientStatus.TryGetValue("web", out var webStatus)
                ? webStatus
                : UserStatus.Offline;

        public bool IsOnMobile => ClientStatus.ContainsKey("mobile");

        internal readonly ConcurrentDictionary<string, UserStatus> ClientStatus;

        internal abstract CachedSharedUser SharedUser { get; }

        internal CachedUser(DiscordClient client, Snowflake id) : base(client, id)
        { }

        internal abstract void Update(UserModel model);

        internal virtual void Update(PresenceUpdateModel model)
        {
            foreach (var (key, value) in model.ClientStatus)
            {

            }
        }

        internal CachedUser Clone()
            => (CachedUser) MemberwiseClone();

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);

        public override string ToString()
            => Tag;
    }
}
