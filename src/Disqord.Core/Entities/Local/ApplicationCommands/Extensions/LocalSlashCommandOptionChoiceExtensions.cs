namespace Disqord
{
    public static class LocalSlashCommandOptionChoiceExtensions
    {
        public static TApplicationCommandOptionChoice WithName<TApplicationCommandOptionChoice>(this TApplicationCommandOptionChoice option, string name)
            where TApplicationCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            option.Name = name;
            return option;
        }

        public static TApplicationCommandOptionChoice WithValue<TApplicationCommandOptionChoice>(this TApplicationCommandOptionChoice option, object value)
            where TApplicationCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            option.Value = value;
            return option;
        }
    }
}
