using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalMessage : ILocalConstruct
    {
        public const int MAX_CONTENT_LENGTH = 2000;

        public const int MAX_NONCE_LENGTH = 25;

        public string Content
        {
            get => _content;
            set
            {
                if (value != null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException(nameof(value), "The message's content must not be empty or whitespace.");

                    if (value.Length > MAX_CONTENT_LENGTH)
                        throw new ArgumentOutOfRangeException(nameof(value), $"The message's content must not be longer than {MAX_CONTENT_LENGTH} characters.");
                }

                _content = value;
            }
        }
        private string _content;
        public bool IsTextToSpeech { get; set; }

        public LocalEmbed Embed { get; set; }

        public LocalAllowedMentions Mentions { get; set; }

        public LocalMessageReference Reference { get; set; }

        public string Nonce
        {
            get => _nonce;
            set
            {
                if (value != null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException(nameof(value), "The message's nonce must not be empty or whitespace.");

                    if (value.Length > MAX_CONTENT_LENGTH)
                        throw new ArgumentOutOfRangeException(nameof(value), $"The message's nonce must not be longer than {MAX_NONCE_LENGTH} characters.");
                }

                _nonce = value;
            }
        }
        private string _nonce;

        public IList<LocalAttachment> Attachments
        {
            get => _attachments;
            set => WithAttachments(value);
        }
        private readonly List<LocalAttachment> _attachments;

        public IList<LocalRowComponent> Components
        {
            get => _components;
            set => WithComponents(value);
        }
        private readonly List<LocalRowComponent> _components;

        public LocalMessage()
        {
            _attachments = new List<LocalAttachment>();
            _components = new List<LocalRowComponent>();
        }

        private LocalMessage(LocalMessage builder)
        {
            Content = builder.Content;
            IsTextToSpeech = builder.IsTextToSpeech;
            Embed = builder.Embed?.Clone();
            Mentions = builder.Mentions?.Clone();
            Reference = builder.Reference?.Clone();
            Nonce = builder.Nonce;
            _attachments = builder.Attachments.ToList();
            _components = builder.Components.ToList();
        }

        public LocalMessage WithContent(string content)
        {
            Content = content;
            return this;
        }

        public LocalMessage WithIsTextToSpeech(bool isTextToSpeech)
        {
            IsTextToSpeech = isTextToSpeech;
            return this;
        }

        public LocalMessage WithEmbed(LocalEmbed embed)
        {
            Embed = embed;
            return this;
        }

        public LocalMessage WithMentions(LocalAllowedMentions mentions)
        {
            Mentions = mentions;
            return this;
        }

        public LocalMessage WithReply(Snowflake messageId, Snowflake? channelId = null, Snowflake? guildId = null, bool failOnInvalid = true)
        {
            Reference ??= new LocalMessageReference();
            Reference.MessageId = messageId;
            Reference.ChannelId = channelId;
            Reference.GuildId = guildId;
            Reference.FailOnInvalid = failOnInvalid;
            return this;
        }

        public LocalMessage WithReference(LocalMessageReference reference)
        {
            Reference = reference;
            return this;
        }

        public LocalMessage WithNonce(string nonce)
        {
            Nonce = nonce;
            return this;
        }

        public LocalMessage WithAttachments(params LocalAttachment[] attachments)
            => WithAttachments(attachments as IEnumerable<LocalAttachment>);

        public LocalMessage WithAttachments(IEnumerable<LocalAttachment> attachments)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            _attachments.Clear();
            _attachments.AddRange(attachments);
            return this;
        }

        public LocalMessage AddAttachment(LocalAttachment attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            _attachments.Add(attachment);
            return this;
        }

        public LocalMessage WithComponents(params LocalRowComponent[] components)
            => WithComponents(components as IEnumerable<LocalRowComponent>);

        public LocalMessage WithComponents(IEnumerable<LocalRowComponent> components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            _components.Clear();
            _components.AddRange(components);
            return this;
        }

        public LocalMessage AddComponent(LocalRowComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _components.Add(component);
            return this;
        }

        public LocalMessage Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (Content == null && Embed == null && _attachments.Count == 0)
                throw new InvalidOperationException("A message must contain at least one of content, embed, or attachments.");

            Embed?.Validate();
            Mentions?.Validate();
            Reference?.Validate();

            // for (var i = 0; i < _attachments.Count; i++)
            // {
            //     _attachments[i].Validate();
            // }
        }
    }
}
