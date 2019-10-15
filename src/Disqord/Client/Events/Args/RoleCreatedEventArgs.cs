namespace Disqord.Events
{
    public sealed class RoleCreatedEventArgs : DiscordEventArgs
    {
        public CachedRole Role { get; }

        internal RoleCreatedEventArgs(CachedRole role) : base(role.Client)
        {
            Role = role;
        }
    }
}
