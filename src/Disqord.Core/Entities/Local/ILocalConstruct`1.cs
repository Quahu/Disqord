namespace Disqord;

/// <summary>
///     Represents a local construct.
/// </summary>
public interface ILocalConstruct<out TSelf> : ILocalConstruct
    where TSelf : ILocalConstruct<TSelf>
{
    /// <summary>
    ///     Clones this instance.
    /// </summary>
    /// <returns>
    ///     A deep copy of this instance.
    /// </returns>
    TSelf Clone();
}
