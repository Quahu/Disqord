using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalWebhookMessageBuilder : ICloneable
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsTextToSpeech { get; set; }

        public LocalAttachment Attachment { get; set; }

        public IList<LocalEmbedBuilder> Embeds
        {
            get => _embeds;
            set => WithEmbeds(value);
        }
        private readonly List<LocalEmbedBuilder> _embeds;

        public LocalMentionsBuilder Mentions { get; set; }

        public LocalWebhookMessageBuilder()
        {
            _embeds = new List<LocalEmbedBuilder>();
        }

        public LocalWebhookMessageBuilder(LocalWebhookMessageBuilder builder)
        {
            Content = builder.Content;
            Name = builder.Name;
            AvatarUrl = builder.AvatarUrl;
            IsTextToSpeech = builder.IsTextToSpeech;
            Attachment = builder.Attachment;
            _embeds = builder.Embeds.ToList();
            Mentions = builder.Mentions?.Clone();
        }

        public LocalWebhookMessageBuilder WithContent(string content)
        {
            Content = content;
            return this;
        }

        public LocalWebhookMessageBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalWebhookMessageBuilder WithAvatarUrl(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
            return this;
        }

        public LocalWebhookMessageBuilder WithIsTextToSpeech(bool isTextToSpeech)
        {
            IsTextToSpeech = isTextToSpeech;
            return this;
        }

        public LocalWebhookMessageBuilder WithAttachment(LocalAttachment attachment)
        {
            Attachment = attachment;
            return this;
        }

        public LocalWebhookMessageBuilder WithEmbeds(params LocalEmbedBuilder[] embeds)
            => WithEmbeds(embeds as IEnumerable<LocalEmbedBuilder>);

        public LocalWebhookMessageBuilder WithEmbeds(IEnumerable<LocalEmbedBuilder> embeds)
        {
            if (embeds == null)
                throw new ArgumentNullException(nameof(embeds));

            _embeds.Clear();
            _embeds.AddRange(embeds);
            return this;
        }

        public LocalWebhookMessageBuilder AddEmbed(LocalEmbedBuilder embed)
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            _embeds.Add(embed);
            return this;
        }

        public LocalWebhookMessageBuilder WithMentions(LocalMentionsBuilder mentions)
        {
            Mentions = mentions;
            return this;
        }

        public virtual LocalWebhookMessageBuilder Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public LocalWebhookMessage Build()
            => new(this);
    }
}
