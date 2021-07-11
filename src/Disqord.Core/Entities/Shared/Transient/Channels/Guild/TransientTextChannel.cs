using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientTextChannel : TransientMessageGuildChannel, ITextChannel
    {
        public string Topic => Model.Topic.Value;

        public bool IsNsfw => Model.Nsfw.Value;

        public bool IsNews => Model.Type == ChannelType.News;

        public TimeSpan DefaultAutomaticArchiveDuration => TimeSpan.FromMinutes(Model.DefaultAutoArchiveDuration.Value);

        public string Mention => Disqord.Mention.TextChannel(this);

        public string Tag => $"#{Name}";

        public TransientTextChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
