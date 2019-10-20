using System.Globalization;

namespace Disqord
{
    public partial interface ICurrentUser : IUser
    {
        CultureInfo Locale { get; }

        bool IsVerified { get; }

        string Email { get; }

        bool HasMfaEnabled { get; }

        string Phone { get; }

        UserFlags Flags { get; }

        bool HasNitro { get; }

        NitroType? NitroType { get; }
    }
}
