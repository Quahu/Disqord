using System;

namespace Disqord
{
    /// <summary>
    ///     Represents mention types in a message's content that will be parsed by Discord.
    /// </summary>
    [Flags]
    public enum ParsedMention
    {
        /// <summary>
        ///     No mentions will be parsed.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Everyone mentions will be parsed.
        /// </summary>
        Everyone = 1,

        /// <summary>
        ///     User mentions will be parsed.
        /// </summary>
        Users = 2,

        /// <summary>
        ///     Role mentions will be parsed.
        /// </summary>
        Roles = 4,

        /// <summary>
        ///     All mentions will be parsed.
        /// </summary>
        All = Everyone | Users | Roles
    }
}
