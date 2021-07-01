using System;

namespace Disqord
{
    public class LocalMessage : LocalMessageBase
    {
        public const int MaxNonceLength = 25;

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

                    if (value.Length > MaxContentLength)
                        throw new ArgumentOutOfRangeException(nameof(value), $"The message's nonce must not be longer than {MaxNonceLength} characters.");
                }

                _nonce = value;
            }
        }
        private string _nonce;

        public LocalMessage()
        { }

        protected LocalMessage(LocalMessage other)
            : base(other)
        {
            Reference = other.Reference?.Clone();
            Nonce = other.Nonce;
        }

        public override LocalMessage Clone()
            => new(this);

        public override void Validate()
        {
            Reference?.Validate();

            base.Validate();
        }
    }
}
