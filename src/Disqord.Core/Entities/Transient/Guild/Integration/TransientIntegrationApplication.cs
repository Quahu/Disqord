using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientIntegrationApplication : TransientClientEntity<IntegrationApplicationJsonModel>, IIntegrationApplication
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public string? IconHash => Model.Icon;

    /// <inheritdoc/>
    public string Description => Model.Description;

    /// <inheritdoc/>
    public string? Summary => Model.Summary;

    /// <inheritdoc/>
    public IUser? Bot => _bot ??= Optional.ConvertOrDefault(Model.Bot, (model, client) => new TransientUser(client, model), Client);

    private TransientUser? _bot;

    public TransientIntegrationApplication(IClient client, IntegrationApplicationJsonModel model)
        : base(client, model)
    { }
}
