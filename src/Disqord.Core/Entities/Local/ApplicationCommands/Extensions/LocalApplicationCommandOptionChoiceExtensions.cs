namespace Disqord
{
    public static class LocalApplicationCommandOptionChoiceExtensions
    {
        public static TApplicationCommandOptionChoice WithName<TApplicationCommandOptionChoice>(this TApplicationCommandOptionChoice option, string name)
            where TApplicationCommandOptionChoice : LocalApplicationCommandOptionChoice
        {
            option.Name = name;
            return option;
        }

        public static TApplicationCommandOptionChoice WithValue<TApplicationCommandOptionChoice>(this TApplicationCommandOptionChoice option, object value)
            where TApplicationCommandOptionChoice : LocalApplicationCommandOptionChoice
        {
            option.Value = value;
            return option;
        }
    }
}
