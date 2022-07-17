using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientGuildWelcomeScreen : TransientClientEntity<WelcomeScreenJsonModel>, IGuildWelcomeScreen
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public string? Description => Model.Description;

    /// <inheritdoc/>
    public IReadOnlyList<IGuildWelcomeScreenChannel> Channels => _channels ??= Model.Channels.ToReadOnlyList(Client,
        (model, client) => new TransientGuildWelcomeScreenChannel(client, model));

    private IReadOnlyList<IGuildWelcomeScreenChannel>? _channels;

    public TransientGuildWelcomeScreen(IClient client, Snowflake guildId, WelcomeScreenJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }
}
