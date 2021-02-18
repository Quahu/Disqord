using System;
using System.Collections.Generic;

namespace Disqord.Gateway
{
    public class ReadyEventArgs : EventArgs
    {
        public ICurrentUser CurrentUser { get; }

        public IReadOnlyList<Snowflake> UnavailableGuildIds { get; }

        public ReadyEventArgs(ICurrentUser currentUser, IReadOnlyList<Snowflake> unavailableGuildIds)
        {
            if (currentUser == null)
                throw new ArgumentNullException(nameof(currentUser));

            if (unavailableGuildIds == null)
                throw new ArgumentNullException(nameof(unavailableGuildIds));

            CurrentUser = currentUser;
            UnavailableGuildIds = unavailableGuildIds;
        }
    }
}
