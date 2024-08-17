using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) retrieved from an application.
/// </summary>
public interface IApplicationEmoji : IOwnedEmoji, IApplicationEntity, IClientEntity, INamableEntity, IJsonUpdatable<EmojiJsonModel>
{ }