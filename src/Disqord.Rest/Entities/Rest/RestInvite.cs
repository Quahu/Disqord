using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestInvite : RestDiscordEntity, IDeletable
    {
        public string Code { get; }

        public RestGuild Guild { get; }

        public RestChannel Channel { get; }

        public int? ApproximateOnlineMemberCount { get; private set; }

        public int? ApproximateMemberCount { get; private set; }

        public RestInviteMetadata Metadata { get; private set; }

        internal RestInvite(RestDiscordClient client, InviteModel model) : base(client)
        {
            Code = model.Code;
            Guild = new RestGuild(client, model.Guild);
            model.Channel.GuildId = model.Guild.Id;
            Channel = RestChannel.Create(client, model.Channel);
            Update(model);
        }

        internal void Update(InviteModel model)
        {
            ApproximateOnlineMemberCount = model.ApproximatePresenceCount;
            ApproximateMemberCount = model.ApproximateMemberCount;

            if (model.Inviter != null) // check for metadata
            {
                if (Metadata == null)
                    Metadata = new RestInviteMetadata(this, model);
                else
                    Metadata.Update(model);
            }
        }

        public Task AcceptAsync(RestRequestOptions options = null)
            => Client.AcceptInviteAsync(Code, options);

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteInviteAsync(Code, options);

        public sealed class RestInviteMetadata
        {
            public RestInvite Invite { get; }

            public RestUser Inviter { get; }

            public int Uses { get; private set; }

            public int MaxUses { get; }

            public TimeSpan MaxAge { get; }

            public bool IsTemporaryMembership { get; }

            public DateTimeOffset CreatedAt { get; }

            public bool IsRevoked { get; private set; }

            internal RestInviteMetadata(RestInvite invite, InviteModel model)
            {
                Invite = invite;
                Inviter = new RestUser(Invite.Client, model.Inviter);
                MaxUses = model.MaxUses;
                MaxAge = TimeSpan.FromSeconds(model.MaxAge);
                IsTemporaryMembership = model.Temporary;
                CreatedAt = model.CreatedAt;
                Update(model);
            }

            internal void Update(InviteModel model)
            {
                Uses = model.Uses;
                IsRevoked = model.Revoked;
            }
        }
    }
}
