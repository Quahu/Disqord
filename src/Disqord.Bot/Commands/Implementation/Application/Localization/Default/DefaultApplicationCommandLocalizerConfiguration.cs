using System.Globalization;

namespace Disqord.Bot.Commands.Application.Default;

public class DefaultApplicationCommandLocalizerConfiguration
{
    public string DirectoryPath { get; set; } = "application-commands/localizations";

    public string LocaleFileNameFormat { get; set; } = "{0}.json";

    public string TemporaryFileNameFormat { get; set; } = "{0}.json.tmp";

    public string BackupFileNameFormat { get; set; } = "{0}.json.bak";

    /// <summary>
    ///     Gets or sets the default culture,
    ///     i.e. the culture which should be populated with the source application command values.
    /// </summary>
    /// <remarks>
    ///     If the default culture is set to <see cref="CultureInfo.InvariantCulture"/>,
    ///     the localizer will not populate any files with source values.
    /// </remarks>
    public CultureInfo DefaultCulture { get; set; } = CultureInfo.CurrentCulture;
}
