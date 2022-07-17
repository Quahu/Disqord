using Disqord.Models;

namespace Disqord;

public class TransientInviteGuild : TransientClientEntity<GuildJsonModel>, IInviteGuild
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public string? SplashHash => Model.Splash;

    /// <inheritdoc/>
    public string? BannerHash => Model.Banner;

    /// <inheritdoc/>
    public string? Description => Model.Description;

    /// <inheritdoc/>
    public string? IconHash => Model.Icon;

    /// <inheritdoc/>
    public GuildFeatures Features => new(Model.Features);

    /// <inheritdoc/>
    public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

    /// <inheritdoc/>
    public string? VanityUrlCode => Model.VanityUrlCode;

    /// <inheritdoc/>
    public IGuildWelcomeScreen WelcomeScreen => _welcomeScreen ??= new TransientGuildWelcomeScreen(Client, Id, Model.WelcomeScreen.Value);

    private IGuildWelcomeScreen? _welcomeScreen;

    /// <inheritdoc/>
    public GuildNsfwLevel NsfwLevel => Model.NsfwLevel;

    public TransientInviteGuild(IClient client, GuildJsonModel model)
        : base(client, model)
    { }
}
