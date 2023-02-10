using System;

namespace Disqord;

[Flags]
public enum MessageFlags
{
    /// <summary>
    ///     The message has no flags.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The message has been crossposted.
    /// </summary>
    Crossposted = 1 << 0,

    /// <summary>
    ///     The message is a crosspost.
    /// </summary>
    Crosspost = 1 << 1,

    /// <summary>
    ///     The message has suppressed embeds.
    /// </summary>
    SuppressedEmbeds = 1 << 2,

    /// <summary>
    ///     The source message, i.e. the crossposted message, has been deleted.
    /// </summary>
    SourceMessageDeleted = 1 << 3,

    /// <summary>
    ///     The message is from the Discord urgent message system.
    /// </summary>
    Urgent = 1 << 4,

    /// <summary>
    ///     The message has a thread associated with it, with the same ID as the message.
    /// </summary>
    HasThread = 1 << 5,

    /// <summary>
    ///     The message is ephemeral, i.e. only visible to the interaction author.
    /// </summary>
    Ephemeral = 1 << 6,

    /// <summary>
    ///     The message is a deferred interaction response.
    /// </summary>
    Loading = 1 << 7,

    /// <summary>
    ///     The message failed to mention some roles and add their members in the thread.
    /// </summary>
    FailedToMentionSomeRolesInThread = 1 << 8,

    /// <summary>
    ///     The message is silent, i.e. does not trigger push and desktop notifications.
    /// </summary>
    SuppressedNotifications = 1 << 12
}
