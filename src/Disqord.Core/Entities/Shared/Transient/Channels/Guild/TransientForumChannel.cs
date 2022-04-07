using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientForumChannel : TransientCategorizableGuildChannel, IForumChannel
    {
        public string Topic => Model.Topic.Value;

        public bool IsNsfw => Model.Nsfw.Value;

        public TimeSpan DefaultAutomaticArchiveDuration => TimeSpan.FromMinutes(Model.DefaultAutoArchiveDuration.Value);

        public TransientForumChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
