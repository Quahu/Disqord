using System;
using System.Globalization;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientUser : TransientClientEntity<UserJsonModel>, IUser
{
    /// <inheritdoc/>
    public virtual Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public virtual string Name => Model.Username;

    /// <inheritdoc/>
    [Obsolete(Pomelo.DiscriminatorObsoletion)]
    public virtual string Discriminator => Model.Discriminator.ToString("0000", CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public virtual string? GlobalName => Model.GlobalName;

    /// <inheritdoc/>
    public virtual string? AvatarHash => Model.Avatar;

    /// <inheritdoc/>
    public virtual bool IsBot => Model.Bot.GetValueOrDefault();

    /// <inheritdoc/>
    public virtual UserFlags PublicFlags => Model.PublicFlags.Value;

    /// <inheritdoc/>
    public virtual string Mention => Disqord.Mention.User(this);

    /// <inheritdoc/>
    public virtual string Tag
    {
        get
        {
            if (this.HasMigratedName())
            {
                // New name system.
                return $"@{Name}";
            }

            // Old name system.
#pragma warning disable CS0618
            return $"{Name}#{Discriminator}";
#pragma warning restore CS0618
        }
    }

    public TransientUser(IClient client, UserJsonModel model)
        : base(client, model)
    { }

    public override string ToString()
    {
        return this.GetString();
    }
}
