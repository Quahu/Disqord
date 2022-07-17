using System.ComponentModel;
using System.Globalization;
using Disqord.Models;

namespace Disqord.Gateway;

public class CachedCurrentUser : CachedShareeUser, ICurrentUser
{
    /// <inheritdoc/>
    public CultureInfo Locale { get; private set; } = null!;

    /// <inheritdoc/>
    public bool IsVerified { get; private set; }

    /// <inheritdoc/>
    public string? Email { get; private set; }

    /// <inheritdoc/>
    public bool HasMfaEnabled { get; private set; }

    /// <inheritdoc/>
    public UserFlags Flags { get; private set; }

    /// <inheritdoc/>
    public NitroType? NitroType { get; private set; }

    public CachedCurrentUser(CachedSharedUser sharedUser, UserJsonModel model)
        : base(sharedUser)
    {
        Update(model);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(UserJsonModel model)
    {
        base.Update(model);

        if (model.Locale.HasValue)
            Locale = Discord.Internal.GetLocale(model.Locale.Value);

        if (model.Verified.HasValue)
            IsVerified = model.Verified.Value;

        if (model.Email.HasValue)
            Email = model.Email.Value;

        if (model.MfaEnabled.HasValue)
            HasMfaEnabled = model.MfaEnabled.Value;

        if (model.Flags.HasValue)
            Flags = model.Flags.Value;

        if (model.PremiumType.HasValue)
            NitroType = model.PremiumType.Value;
    }
}
