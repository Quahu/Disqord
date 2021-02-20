using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalMessageBuilder : ICloneable
    {
        public string Content { get; set; }

        public bool IsTextToSpeech { get; set; }

        public LocalEmbedBuilder Embed { get; set; }

        public LocalMentionsBuilder Mentions { get; set; }

        public LocalReferenceBuilder Reference { get; set; }

        public string Nonce { get; set; }

        public IList<LocalAttachment> Attachments
        {
            get => _attachments;
            set => WithAttachments(value);
        }

        private readonly List<LocalAttachment> _attachments;

        public LocalMessageBuilder()
        {
            _attachments = new List<LocalAttachment>();
        }

        public LocalMessageBuilder(LocalMessageBuilder builder)
        {
            Content = builder.Content;
            IsTextToSpeech = builder.IsTextToSpeech;
            Embed = builder.Embed?.Clone();
            Mentions = builder.Mentions?.Clone();
            Reference = builder.Reference?.Clone();
            Nonce = builder.Nonce;
            _attachments = builder.Attachments.ToList();
        }

        public LocalMessageBuilder WithContent(string content)
        {
            Content = content;
            return this;
        }

        public LocalMessageBuilder WithIsTextToSpeech(bool isTextToSpeech)
        {
            IsTextToSpeech = isTextToSpeech;
            return this;
        }

        public LocalMessageBuilder WithEmbed(LocalEmbedBuilder embed)
        {
            Embed = embed;
            return this;
        }

        public LocalMessageBuilder WithMentions(LocalMentionsBuilder mentions)
        {
            Mentions = mentions;
            return this;
        }

        public LocalMessageBuilder WithReply(Snowflake messageId, Snowflake? channelId = null, Snowflake? guildId = null, bool failOnInvalid = true)
        {
            Reference ??= new LocalReferenceBuilder();
            Reference.MessageId = messageId;
            Reference.ChannelId = channelId;
            Reference.GuildId = guildId;
            Reference.FailOnInvalid = failOnInvalid;
            return this;
        }

        public LocalMessageBuilder WithReference(LocalReferenceBuilder reference)
        {
            Reference = reference;
            return this;
        }

        public LocalMessageBuilder WithNonce(string nonce)
        {
            Nonce = nonce;
            return this;
        }

        public LocalMessageBuilder WithAttachments(params LocalAttachment[] attachments)
            => WithAttachments(attachments as IEnumerable<LocalAttachment>);

        public LocalMessageBuilder WithAttachments(IEnumerable<LocalAttachment> attachments)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            _attachments.Clear();
            _attachments.AddRange(attachments);
            return this;
        }

        public LocalMessageBuilder AddAttachment(LocalAttachment attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            _attachments.Add(attachment);
            return this;
        }

        public LocalMessageBuilder Clone()
            => new LocalMessageBuilder(this);

        object ICloneable.Clone()
            => Clone();

        public LocalMessage Build()
        {
            return new LocalMessage(this);
        }
    }
}
