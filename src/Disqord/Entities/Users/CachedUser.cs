using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest;
using PresenceUpdateDispatch = Disqord.Models.Dispatches.PresenceUpdateModel;

namespace Disqord
{
    public abstract class CachedUser : CachedSnowflakeEntity, IUser
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

        internal virtual void Update(PresenceUpdateDispatch model)
        {
            foreach (var (key, value) in model.ClientStatus)
            {

            }
        }

        internal CachedUser Clone()
            => (CachedUser) MemberwiseClone();

        public string GetAvatarUrl(ImageFormat? imageFormat = null, int size = 2048)
            => AvatarHash != null
                ? Discord.GetUserAvatarUrl(Id, AvatarHash, imageFormat, size)
                : Discord.GetDefaultUserAvatarUrl(Discriminator);

        public Task SetNoteAsync(string note, RestRequestOptions options = null)
            => Client.SetNoteAsync(Id, note, options);

        public async Task<IDmChannel> GetOrCreateDMChannelAsync(RestRequestOptions options = null)
        {
            var channel = Client.DmChannels.Values.FirstOrDefault(x => x.Recipient.Id == Id);
            return channel ?? (IDmChannel) await Client.CreateDmChannelAsync(Id, options).ConfigureAwait(false);
        }

        public Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null)
            => Client.CreateDmChannelAsync(Id, options);

        public async Task<RestUserMessage> SendMessageAsync(string content = null, bool isTts = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await GetOrCreateDMChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(content, isTts, embed, options).ConfigureAwait(false);
        }

        public async Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTts = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await GetOrCreateDMChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(attachment, content, isTts, embed, options).ConfigureAwait(false);
        }

        public async Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTts = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await GetOrCreateDMChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(attachments, content, isTts, embed, options).ConfigureAwait(false);
        }

        public override string ToString()
            => Tag;
    }
}
