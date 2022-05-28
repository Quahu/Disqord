namespace Disqord.Bot.Commands.Application.Default;

public class DefaultApplicationCommandCacheProviderConfiguration
{
    public string DirectoryPath { get; set; } = "application-commands/cache";

    public string FileName { get; set; } = "cache";

    public string FileNameFormat { get; set; } = "{0}.json";

    public string TemporaryFileNameFormat { get; set; } = "{0}.json.tmp";

    public string BackupFileNameFormat { get; set; } = "{0}.json.bak";
}
