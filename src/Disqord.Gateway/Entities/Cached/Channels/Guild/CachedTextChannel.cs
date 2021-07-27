using System;
using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedTextChannel : CachedMessageGuildChannel, ITextChannel
    {
        public string Topic { get; private set; }

        public bool IsNsfw { get; private set; }

        public bool IsNews => Type == ChannelType.News;

        public TimeSpan DefaultAutomaticArchiveDuration { get; private set; }

        public CachedTextChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Topic.HasValue)
                Topic = model.Topic.Value;

            if (model.Nsfw.HasValue)
                IsNsfw = model.Nsfw.Value;

            if (model.DefaultAutoArchiveDuration.HasValue)
                DefaultAutomaticArchiveDuration = TimeSpan.FromMinutes(model.DefaultAutoArchiveDuration.Value);
        }
    }
}
