using System;

namespace Disqord;

/// <summary>
///     Provides utilities related to the Disqord library.
/// </summary>
public static partial class Library
{
    /// <summary>
    ///     Disqord build's version.
    /// </summary>
    public static readonly Version Version = typeof(Library).Assembly.GetName().Version!;

    /// <summary>
    ///     Disqord build's date.
    /// </summary>
    public static readonly DateTimeOffset BuiltAt = DateTimeOffset.UtcNow;

    /// <summary>
    ///     Disqord's repository url.
    /// </summary>
    public const string RepositoryUrl = "https://github.com/Quahu/Disqord";

    /// <summary>
    ///     Disqord's user agent.
    /// </summary>
    public static readonly string UserAgent = $"DiscordBot ({RepositoryUrl}, {Version.ToString(3)})";
}
