namespace Disqord
{
    public sealed class CreateRoleProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<GuildPermissions> Permissions { internal get; set; }

        public Optional<Color> Color { internal get; set; }

        public Optional<bool> IsHoisted { internal get; set; }

        public Optional<bool> IsMentionable { internal get; set; }

        internal CreateRoleProperties()
        { }
    }
}
