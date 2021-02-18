using Disqord.Models;

namespace Disqord
{
    public class TransientEmoji : TransientEntity<EmojiJsonModel>, IEmoji
    {
        public string Name => Model.Name;

        public TransientEmoji(IClient client, EmojiJsonModel model)
            : base(client, model)
        { }

        public override int GetHashCode()
            => Name.GetHashCode();

        public virtual bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override string ToString()
            => Name;

        /// <summary>
        ///     Creates either a <see cref="TransientEmoji"/> or a <see cref="TransientCustomEmoji"/> based on the presence of ID.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEmoji Create(IClient client, EmojiJsonModel model)
        {
            if (model.Id != null)
                return new TransientCustomEmoji(client, model);

            return new TransientEmoji(client, model);
        }
    }
}
