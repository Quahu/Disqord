using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientInviteMetadata : TransientEntity<InviteJsonModel>, IInviteMetadata
    {
        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Model.CreatedAt.Value;

        /// <inheritdoc/>
        public TimeSpan MaxAge => TimeSpan.FromSeconds(Model.MaxAge.Value);

        /// <inheritdoc/>
        public int MaxUses => Model.MaxUses.Value;

        /// <inheritdoc/>
        public int Uses => Model.Uses.Value;

        /// <inheritdoc/>
        public bool IsTemporaryMembership => Model.Temporary.Value;

        public TransientInviteMetadata(InviteJsonModel model)
            : base(model)
        { }
    }
}
