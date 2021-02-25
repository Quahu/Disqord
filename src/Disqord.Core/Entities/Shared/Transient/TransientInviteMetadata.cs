using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientInviteMetadata : TransientEntity<InviteJsonModel>, IInviteMetadata
    {
        public DateTimeOffset CreatedAt => Model.CreatedAt.Value;

        public IUser Inviter
        {
            get
            {
                if (_inviter == null)
                    _inviter = new TransientUser(Client, Model.Inviter.Value!);

                return _inviter;
            }
        }
        private IUser? _inviter;

        public TimeSpan MaxAge => TimeSpan.FromSeconds(Model.MaxAge.Value);

        public int MaxUses => Model.MaxUses.Value;

        public int Uses => Model.Uses.Value;

        public bool IsTemporaryMembership => Model.Temporary.Value;

        public TransientInviteMetadata(IClient client, InviteJsonModel model)
            : base(client, model)
        { }
    }
}
