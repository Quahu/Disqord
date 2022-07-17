namespace Disqord.Rest;

/// <summary>
///     Represents the direction in which entities should be fetched.
/// </summary>
public enum FetchDirection
{
    /// <summary>
    ///     Entities should be fetched before the specified value.
    /// </summary>
    Before,

    /// <summary>
    ///     Entities should be fetched around the specified value.
    /// </summary>
    /// <remarks>
    ///     <b>Support for this value in the library might be limited.</b>
    /// </remarks>
    Around,

    /// <summary>
    ///     Entities should be fetched after the specified value.
    /// </summary>
    After
}
