namespace Disqord
{
    public static class LocalApplicationCommandExtensions
    {
        public static TApplicationCommand WithName<TApplicationCommand>(this TApplicationCommand command, string name)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.Name = name;
            return command;
        }

        public static TApplicationCommand WithIsEnabledByDefault<TApplicationCommand>(this TApplicationCommand command, bool isEnabledByDefault = true)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.IsEnabledByDefault = isEnabledByDefault;
            return command;
        }
    }
}
