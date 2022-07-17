namespace Disqord;

/// <summary>
///     Represents a message channel in a guild.
/// </summary>
public interface IMessageGuildChannel : IMessageChannel, ISlowmodeChannel, ITaggableEntity
{ }