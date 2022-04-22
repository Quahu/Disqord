using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientGuildSticker : TransientSticker, IGuildSticker
    {
        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId.Value;

        /// <inheritdoc/>
        public bool IsAvailable => Model.Available.GetValueOrDefault();

        /// <inheritdoc/>
        public IUser Creator => _creator ??= new TransientUser(Client, Model.User.Value);
        private IUser _creator;

        public TransientGuildSticker(IClient client, StickerJsonModel model)
            : base(client, model)
        { }
    }
}