using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientGuildEventMetadata : TransientEntity<GuildScheduledEventEntityMetadataJsonModel>, IGuildEventMetadata
    {
        public string Location => Model.Location.GetValueOrDefault();

        public TransientGuildEventMetadata(GuildScheduledEventEntityMetadataJsonModel model)
            : base(model)
        { }
    }
}
