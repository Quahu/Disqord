namespace Disqord;

public static class LocalFileComponentExtensions
{
    public static TFileComponent WithFile<TFileComponent>(this TFileComponent component, string url)
        where TFileComponent : LocalFileComponent
    {
        return component.WithFile(new LocalUnfurledMediaItem(url));
    }

    public static TFileComponent WithFile<TFileComponent>(this TFileComponent component, LocalUnfurledMediaItem file)
        where TFileComponent : LocalFileComponent
    {
        component.File = file;
        return component;
    }
}
