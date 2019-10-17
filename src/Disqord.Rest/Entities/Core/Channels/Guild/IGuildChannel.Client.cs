﻿using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IGuildChannel : IChannel, IDeletable
    {
        Task AddOrModifyOverwriteAsync(LocalOverwrite overwrite, RestRequestOptions options = null);

        Task DeleteOverwriteAsync(Snowflake targetId, RestRequestOptions options = null);
    }
}
