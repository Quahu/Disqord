namespace Disqord.Events
{
    public sealed class RoleUpdatedEventArgs : DiscordEventArgs
    {
        public CachedRole OldRole { get; }

        public CachedRole NewRole { get; }

        internal RoleUpdatedEventArgs(CachedRole oldRole, CachedRole newRole) : base(newRole.Client)
        {
            OldRole = oldRole;
            NewRole = newRole;
        }
    }
}
