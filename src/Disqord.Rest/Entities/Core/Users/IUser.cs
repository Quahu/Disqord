namespace Disqord
{
    public partial interface IUser : IMessagable, IMentionable, ITaggable
    {
        string Name { get; }

        string Discriminator { get; }

        string AvatarHash { get; }

        bool IsBot { get; }

        string GetAvatarUrl(ImageFormat format = default, int size = 2048);
    }
}
