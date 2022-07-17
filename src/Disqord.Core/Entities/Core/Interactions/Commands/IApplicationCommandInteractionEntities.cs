using System.Collections.Generic;

namespace Disqord;

// TODO: expose member permissions depending on how select menus v2 handle things
/// <summary>
///     Represents resolved entities for application command options.
/// </summary>
public interface IApplicationCommandInteractionEntities
{
    /// <summary>
    ///     Gets the resolved users/members for the interaction.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IUser> Users { get; }

    /// <summary>
    ///     Gets the resolved roles for the interaction.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

    /// <summary>
    ///     Gets the resolved channels for the interaction.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IInteractionChannel> Channels { get; }

    /// <summary>
    ///     Gets the resolved messages for the interaction.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IMessage> Messages { get; }

    /// <summary>
    ///     Gets the resolved attachments for the interaction.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IAttachment> Attachments { get; }
}