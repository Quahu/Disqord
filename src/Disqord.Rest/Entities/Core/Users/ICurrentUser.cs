namespace Disqord
{
    public partial interface ICurrentUser : IUser
    {
        string Locale { get; }

        bool IsVerified { get; }

        string Email { get; }

        bool HasMfaEnabled { get; }
    }
}
