namespace Disqord;

public static class LocalFileUploadComponentExtensions
{
    public static TFileUploadComponent WithCustomId<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, string customId)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        fileUploadComponent.CustomId = customId;
        return fileUploadComponent;
    }

    public static TFileUploadComponent WithMinimumUploadedFiles<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, int minimumUploadedFiles)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        fileUploadComponent.MinimumUploadedFiles = minimumUploadedFiles;
        return fileUploadComponent;
    }

    public static TFileUploadComponent WithMaximumUploadedFiles<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, int maximumUploadedFiles)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        fileUploadComponent.MaximumUploadedFiles = maximumUploadedFiles;
        return fileUploadComponent;
    }

    public static TFileUploadComponent WithIsRequired<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, bool isRequired = true)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        fileUploadComponent.IsRequired = isRequired;
        return fileUploadComponent;
    }
}
