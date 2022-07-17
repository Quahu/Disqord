using Disqord.Models;

namespace Disqord;

public class TransientMessageInteraction : TransientEntity<MessageInteractionJsonModel>, IMessageInteraction
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public InteractionType Type => Model.Type;

    /// <inheritdoc/>
    public IUser Author { get; }

    /// <inheritdoc/>
    public TransientMessageInteraction(IUser author, MessageInteractionJsonModel model)
        : base(model)
    {
        Author = author;
    }
}
