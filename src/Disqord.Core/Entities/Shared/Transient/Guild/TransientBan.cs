using Disqord.Models;

namespace Disqord
{
    public class TransientBan : TransientEntity<BanJsonModel>, IBan
    {
        public Snowflake GuildId { get; }

        public IUser User
        {
            get
            {
                if (_user == null)
                    _user = new TransientUser(Client, Model.User);

                return _user;
            }
        }
        private TransientUser _user;

        public string Reason => Model.Reason;

        public TransientBan(IClient client, Snowflake guildId, BanJsonModel model) : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
