using System.Collections.Generic;
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

        public virtual CachedRelationship Relationship => Client.CurrentUser.Relationships.GetValueOrDefault(Id);

        public string Note => Client.CurrentUser.Notes.GetValueOrDefault(Id);

        public virtual CachedDmChannel DmChannel => Client.DmChannels.Values.FirstOrDefault(x => x.Recipient.Id == Id);

        public virtual UserStatus Status => SharedUser.Status;

        public virtual IReadOnlyDictionary<UserClient, UserStatus> Statuses => SharedUser.Statuses;

        public virtual Activity Activity => SharedUser.Activity;

        public virtual IReadOnlyList<Activity> Activities => SharedUser.Activities;

        internal abstract CachedSharedUser SharedUser { get; }

        internal CachedUser(CachedSharedUser sharedUser) : base(sharedUser.Client, sharedUser.Id)
        { }

        internal CachedUser(DiscordClientBase client, UserModel model) : base(client, model.Id)
        {
            IsBot = model.Bot;
        }

        internal virtual void Update(UserModel model)
        {
            if (model.Username.HasValue)
                Name = model.Username.Value;

            if (model.Discriminator.HasValue)
                Discriminator = model.Discriminator.Value;

            if (model.Avatar.HasValue)
                AvatarHash = model.Avatar.Value;
        }

        internal virtual void Update(PresenceUpdateModel model)
            => SharedUser.Update(model);

        internal CachedUser Clone()
            => (CachedUser) MemberwiseClone();

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);

        public override string ToString()
            => Tag;
    }
}
