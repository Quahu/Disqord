using System.Collections.Generic;
using Qommon;

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

    public static TFileUploadComponent AddFileType<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, string fileType)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        Guard.IsNotNullOrWhiteSpace(fileType);

        if (fileUploadComponent.FileTypes.Add(fileType, out var list))
        {
            fileUploadComponent.FileTypes = new(list);
        }

        return fileUploadComponent;
    }

    public static TFileUploadComponent WithFileTypes<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, IEnumerable<string> fileTypes)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        Guard.IsNotNull(fileTypes);

        if (fileUploadComponent.FileTypes.With(fileTypes, out var list))
        {
            fileUploadComponent.FileTypes = new(list);
        }

        return fileUploadComponent;
    }

    public static TFileUploadComponent WithFileTypes<TFileUploadComponent>(this TFileUploadComponent fileUploadComponent, params string[] fileTypes)
        where TFileUploadComponent : LocalFileUploadComponent
    {
        return fileUploadComponent.WithFileTypes(fileTypes as IEnumerable<string>);
    }
}
