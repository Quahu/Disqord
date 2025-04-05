namespace Disqord;

/// <summary>
///     Represents the type of a message reference.
/// </summary>
public enum MessageReferenceType
{
    /// <summary>
    ///     A standard reference used by replies.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     A reference used to point to a message at a point in time.
    /// </summary>
    Forward = 1
}