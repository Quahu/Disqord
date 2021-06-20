using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public abstract class LocalMessageBase : ILocalConstruct
    {
        public const int MAX_CONTENT_LENGTH = 2000;

        public const int MAX_EMBEDS_LENGTH = 6000;

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

        public IList<LocalEmbed> Embeds
        {
            get => _embeds;
            set => this.WithEmbeds(value);
        }
        internal readonly List<LocalEmbed> _embeds;

        public LocalAllowedMentions AllowedMentions { get; set; }

        public IList<LocalAttachment> Attachments
        {
            get => _attachments;
            set => this.WithAttachments(value);
        }
        internal readonly List<LocalAttachment> _attachments;

        public IList<LocalRowComponent> Components
        {
            get => _components;
            set => this.WithComponents(value);
        }
        internal readonly List<LocalRowComponent> _components;

        protected LocalMessageBase()
        {
            _embeds = new List<LocalEmbed>();
            _attachments = new List<LocalAttachment>();
            _components = new List<LocalRowComponent>();
        }

        protected LocalMessageBase(LocalMessageBase other)
        {
            Content = other.Content;
            IsTextToSpeech = other.IsTextToSpeech;
            _embeds = other._embeds.Select(x => x.Clone()).ToList();
            AllowedMentions = other.AllowedMentions?.Clone();
            _attachments = other._attachments.Select(x => x.Clone()).ToList();
            _components = other._components.Select(x => x.Clone()).ToList();
        }

        public abstract LocalMessageBase Clone();

        object ICloneable.Clone()
            => Clone();

        public virtual void Validate()
        {
            if (Content == null && _embeds.Count == 0 && _attachments.Count == 0)
                throw new InvalidOperationException("A message must contain at least one of content, embeds, or attachments.");

            if (_embeds.Sum(x => x.Length) > MAX_EMBEDS_LENGTH)
                throw new InvalidOperationException($"The total length of embeds must not exceed {MAX_EMBEDS_LENGTH} characters.");

            for (var i = 0; i < _embeds.Count; i++)
                _embeds[i].Validate();

            AllowedMentions?.Validate();

            for (var i = 0; i < _attachments.Count; i++)
                _attachments[i].Validate();

            for (var i = 0; i < _components.Count; i++)
                _components[i].Validate();
        }
    }
}
