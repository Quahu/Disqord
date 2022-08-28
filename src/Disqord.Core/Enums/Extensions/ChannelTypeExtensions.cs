using System.ComponentModel;

namespace Disqord;

/// <summary>
///     Represents <see cref="ChannelType"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ChannelTypeExtensions
{
    /// <summary>
    ///     Gets whether <paramref name="type"/> represents a thread channel type.
    /// </summary>
    /// <param name="type"> The type. </param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="type"/> is a thread channel type.
    /// </returns>
    public static bool IsThread(this ChannelType type)
    {
        return type is ChannelType.NewsThread or ChannelType.PublicThread or ChannelType.PublicThread;
    }
}
