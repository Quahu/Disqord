using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildWelcomeScreen : TransientEntity<WelcomeScreenJsonModel>, IGuildWelcomeScreen
    {
        public string Description => Model.Description;

        public IReadOnlyList<IGuildWelcomeScreenChannel> Channels => _channels ??= Model.Channels.ToReadOnlyList(x => new TransientGuildWelcomeScreenChannel(Client, x));
        private IReadOnlyList<IGuildWelcomeScreenChannel> _channels;

        public TransientGuildWelcomeScreen(IClient client, WelcomeScreenJsonModel model)
            : base(client, model)
        { }

    }
}
