namespace Disqord.Serialization.Json;

/// <summary>
///     Represents how null values should be handled by the serializer.
/// </summary>
public enum NullValueHandling
{
    /// <summary>
    ///     Null values should be included.
    /// </summary>
    Include = 0,

    /// <summary>
    ///     Null values should be ignored.
    /// </summary>
    Ignore = 1
}