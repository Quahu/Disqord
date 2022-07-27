using System.Collections.Generic;
using Disqord.Rest;

namespace Disqord.Bot;

public class DiscordBotBaseConfiguration
{
    /// <summary>
    ///     Gets or sets the IDs of the users that own this bot,
    ///     i.e. users who will pass <see cref="DiscordBotBase.IsOwnerAsync"/>.
    /// </summary>
    /// <remarks>
    ///     This can be set to save on a REST call to <see cref="RestClientExtensions.FetchCurrentApplicationAsync"/>
    ///     when <see cref="DiscordBotBase.IsOwnerAsync"/> is called for the first time.
    /// </remarks>
    public IEnumerable<Snowflake>? OwnerIds { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the bot's Discord application.
    /// </summary>
    /// <remarks>
    ///     This can be set to save on REST call to <see cref="RestClientExtensions.FetchCurrentApplicationAsync"/>
    ///     when <see cref="DiscordBotBase.SyncApplicationCommands"/> is called for the first time.
    /// </remarks>
    public Snowflake? ApplicationId { get; set; }

    /// <summary>
    ///     Gets or sets whether the bot should automatically
    ///     sync global application commands.
    /// </summary>
    /// <remarks>
    ///     This is checked after <see cref="DiscordBotBase.ShouldInitializeApplicationCommands"/>.
    /// </remarks>
    public bool SyncGlobalApplicationCommands { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether the bot should automatically
    ///     sync guild application commands.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SyncGlobalApplicationCommands"/>
    /// </remarks>
    public bool SyncGuildApplicationCommands { get; set; } = true;
}
