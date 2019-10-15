using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestMutualGuild : RestSnowflakeEntity
    {
        public string Nick { get; private set; }

        internal RestMutualGuild(RestDiscordClient client, MutualGuildModel model) : base(client, model.Id)
        {
            Update(model);
        }

        internal void Update(MutualGuildModel model)
        {
            Nick = model.Nick;
        }
    }
}
