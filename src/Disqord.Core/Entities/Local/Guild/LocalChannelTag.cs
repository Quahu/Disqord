using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local channel tag to be created within a forum or media channel.
/// </summary>
/// <remarks>
///     Note that when updating a channel's tags,
///     you must provide the previous tags with their original values.
///     You can use <see cref="CreateFrom"/> for that purpose.
/// </remarks>
public class LocalChannelTag : ILocalConstruct<LocalChannelTag>, IJsonConvertible<ChannelTagJsonModel>
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
    ///     Instantiates a new <see cref="LocalChannelTag"/>.
    /// </summary>
    public LocalChannelTag()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalChannelTag"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalChannelTag(LocalChannelTag other)
    {
        Id = other.Id;
        Name = other.Name;
        IsModerated = other.IsModerated;
        Emoji = other.Emoji.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalChannelTag Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public ChannelTagJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);

        var model = new ChannelTagJsonModel
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
    ///     Converts the specified channel tag to a <see cref="LocalChannelTag"/>.
    /// </summary>
    /// <param name="tag"> The channel tag to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalChannelTag"/>.
    /// </returns>
    public static LocalChannelTag CreateFrom(IChannelTag tag)
    {
        return new LocalChannelTag
        {
            Id = tag.Id,
            Name = tag.Name,
            IsModerated = tag.IsModerated,
            Emoji = LocalEmoji.FromEmoji(tag.Emoji)!
        };
    }
}
