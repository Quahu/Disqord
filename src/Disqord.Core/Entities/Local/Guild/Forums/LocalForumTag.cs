using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local forum tag to be created within a forum channel.
/// </summary>
/// <remarks>
///     Note that when updating a forum channel's tags,
///     you must provide the previous tags with their original values.
///     You can use <see cref="CreateFrom"/> for that purpose.
/// </remarks>
public class LocalForumTag : ILocalConstruct<LocalForumTag>, IJsonConvertible<ForumTagJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of this tag.
    /// </summary>
    /// <remarks>
    ///     This property is required when modifying an existing tag.
    /// </remarks>
    public Optional<Snowflake> Id { get; set; }

    /// <summary>
    ///     Gets or sets the name of this tag.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets whether this tag can only be applied to threads
    ///     by members with the <see cref="Permissions.ManageThreads"/> permission.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="false"/>.
    /// </remarks>
    public Optional<bool> IsModerated { get; set; }

    /// <summary>
    ///     Gets or sets the emoji of this tag.
    /// </summary>
    public Optional<LocalEmoji> Emoji { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalForumTag"/>.
    /// </summary>
    public LocalForumTag()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalForumTag"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalForumTag(LocalForumTag other)
    {
        Id = other.Id;
        Name = other.Name;
        IsModerated = other.IsModerated;
        Emoji = other.Emoji.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalForumTag Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public ForumTagJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);

        var model = new ForumTagJsonModel
        {
            Id = Id,
            Name = Name.Value,
            Moderated = IsModerated.GetValueOrDefault()
        };

        var emoji = Emoji.GetValueOrDefault();
        if (emoji != null)
        {
            if (emoji is LocalCustomEmoji customEmoji)
            {
                OptionalGuard.HasValue(customEmoji.Id);

                model.EmojiId = customEmoji.Id.Value;
            }
            else
            {
                OptionalGuard.HasValue(emoji.Name);
                Guard.IsNotNull(emoji.Name.Value);

                model.EmojiName = emoji.Name.Value;
            }
        }

        return model;
    }

    /// <summary>
    ///     Converts the specified forum tag to a <see cref="LocalForumTag"/>.
    /// </summary>
    /// <param name="tag"> The forum tag to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalForumTag"/>.
    /// </returns>
    public static LocalForumTag CreateFrom(IForumTag tag)
    {
        return new LocalForumTag
        {
            Id = tag.Id,
            Name = tag.Name,
            IsModerated = tag.IsModerated,
            Emoji = LocalEmoji.FromEmoji(tag.Emoji)!
        };
    }
}
