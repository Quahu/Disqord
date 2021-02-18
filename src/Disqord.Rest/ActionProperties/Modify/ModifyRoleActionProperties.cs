namespace Disqord
{
    public sealed class ModifyRoleActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<GuildPermissions> Permissions { internal get; set; }

        public Optional<Color?> Color { internal get; set; }

        public Optional<bool> IsHoisted { internal get; set; }

        public Optional<bool> IsMentionable { internal get; set; }

        public Optional<int> Position { internal get; set; }

        internal ModifyRoleActionProperties()
        { }

        internal bool HasValues
            => Name.HasValue || Permissions.HasValue || Color.HasValue || IsHoisted.HasValue || IsMentionable.HasValue || Position.HasValue;
    }
}
