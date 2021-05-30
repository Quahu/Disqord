using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;

namespace Disqord
{
    public class LocalWebhookMessage : ILocalConstruct
    {
        public string Content
        {
            get => _content;
            set
            {
                if (value != null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException(nameof(value), "The message's content must not be empty or whitespace.");

                    if (value.Length > LocalMessage.MAX_CONTENT_LENGTH)
                        throw new ArgumentOutOfRangeException(nameof(value), $"The message's content must not be longer than {LocalMessage.MAX_CONTENT_LENGTH} characters.");
                }

                _content = value;
            }
        }
        private string _content;

        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsTextToSpeech { get; set; }

        public LocalAttachment Attachment { get; set; }

        public IList<LocalEmbed> Embeds
        {
            get => _embeds;
            set => WithEmbeds(value);
        }
        private readonly List<LocalEmbed> _embeds;

        public LocalAllowedMentions Mentions { get; set; }

        public LocalWebhookMessage()
        {
            _embeds = new List<LocalEmbed>();
        }

        private LocalWebhookMessage(LocalWebhookMessage other)
        {
            Content = other.Content;
            Name = other.Name;
            AvatarUrl = other.AvatarUrl;
            IsTextToSpeech = other.IsTextToSpeech;
            Attachment = other.Attachment;
            Embeds = other._embeds.Select(x => x.Clone()).ToList();
            Mentions = other.Mentions?.Clone();
        }

        public LocalWebhookMessage WithContent(string content)
        {
            Content = content;
            return this;
        }

        public LocalWebhookMessage WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalWebhookMessage WithAvatarUrl(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
            return this;
        }

        public LocalWebhookMessage WithIsTextToSpeech(bool isTextToSpeech)
        {
            IsTextToSpeech = isTextToSpeech;
            return this;
        }

        public LocalWebhookMessage WithAttachment(LocalAttachment attachment)
        {
            Attachment = attachment;
            return this;
        }

        public LocalWebhookMessage WithEmbeds(params LocalEmbed[] embeds)
            => WithEmbeds(embeds as IEnumerable<LocalEmbed>);

        public LocalWebhookMessage WithEmbeds(IEnumerable<LocalEmbed> embeds)
        {
            if (embeds == null)
                throw new ArgumentNullException(nameof(embeds));

            _embeds.Clear();
            _embeds.AddRange(embeds);
            return this;
        }

        public LocalWebhookMessage AddEmbed(LocalEmbed embed)
        {
            if (embed == null)
                throw new ArgumentNullException(nameof(embed));

            _embeds.Add(embed);
            return this;
        }

        public LocalWebhookMessage WithMentions(LocalAllowedMentions mentions)
        {
            Mentions = mentions;
            return this;
        }

        public virtual LocalWebhookMessage Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (Content == null && Embeds.Count == 0 && Attachment == null)
                throw new InvalidOperationException("A webhook message must contain at least one of content, embeds, or attachment.");
        }
    }
}
