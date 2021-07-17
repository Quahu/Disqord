using Disqord.Models;

namespace Disqord
{
    public class TransientGuildSticker : TransientSticker, IGuildSticker
    {
        public Snowflake GuildId => Model.GuildId.Value;

        public bool IsAvailable => Model.Available.GetValueOrDefault();

        public IUser Creator => _creator ??= new TransientUser(Client, Model.User.Value);
        private IUser _creator;

        public TransientGuildSticker(IClient client, StickerJsonModel model)
            : base(client, model)
        { }
    }
}