namespace Disqord
{
    public static class LocalSlashCommandOptionChoiceExtensions
    {
        public static TSlashCommandOptionChoice WithName<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, string name)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            @this.Name = name;
            return @this;
        }

        public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, long value)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            @this.Value = value;
            return @this;
        }

        public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, double value)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            @this.Value = value;
            return @this;
        }

        public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, string value)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            @this.Value = value;
            return @this;
        }
    }
}
