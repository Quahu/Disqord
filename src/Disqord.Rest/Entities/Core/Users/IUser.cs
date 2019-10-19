namespace Disqord
{
    public partial interface IUser : IMessagable, IMentionable, ITaggable
    {
        string Name { get; }

        string Discriminator { get; }

        string AvatarHash { get; }

        bool IsBot { get; }

        string GetAvatarUrl(ImageFormat? imageFormat = null, int size = 2048);
    }
}
