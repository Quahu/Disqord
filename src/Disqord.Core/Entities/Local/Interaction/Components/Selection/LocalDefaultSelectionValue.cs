using Disqord.Models;

namespace Disqord;

public class LocalDefaultSelectionValue : IDefaultSelectionValue, ILocalConstruct<LocalDefaultSelectionValue>, IJsonConvertible<DefaultValueJsonModel>
{
    public static LocalDefaultSelectionValue User(Snowflake id)
    {
        return new(id, DefaultSelectionValueType.User);
    }

    public static LocalDefaultSelectionValue Role(Snowflake id)
    {
        return new(id, DefaultSelectionValueType.Role);
    }

    public static LocalDefaultSelectionValue Channel(Snowflake id)
    {
        return new(id, DefaultSelectionValueType.Channel);
    }

    /// <inheritdoc/>
    public Snowflake Id { get; set; }

    /// <inheritdoc/>
    public DefaultSelectionValueType Type { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalDefaultSelectionValue"/>.
    /// </summary>
    public LocalDefaultSelectionValue()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalDefaultSelectionValue"/>.
    /// </summary>
    /// <param name="id"> The id of the entity. </param>
    /// <param name="type"> The type of the entity. </param>
    public LocalDefaultSelectionValue(Snowflake id, DefaultSelectionValueType type)
    {
        Id = id;
        Type = type;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalDefaultSelectionValue"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalDefaultSelectionValue(LocalDefaultSelectionValue other)
    {
        Id = other.Id;
        Type = other.Type;
    }

    /// <inheritdoc/>
    public LocalDefaultSelectionValue Clone()
    {
        return new(this);
    }

    public DefaultValueJsonModel ToModel()
    {
        return new DefaultValueJsonModel
        {
            Id = Id,
            Type = Type
        };
    }
}
