namespace Disqord
{
    public static class LocalSlashCommandOptionChoiceExtensions
    {
        public static TSlashCommandOptionChoice WithName<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice option, string name)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            option.Name = name;
            return option;
        }

        public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice option, object value)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            option.Value = value;
            return option;
        }
    }
}
