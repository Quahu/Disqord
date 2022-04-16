namespace Disqord
{
    /// <summary>
    ///     Represents the type of an interaction.
    /// </summary>
    public enum InteractionType
    {
        Ping = 1,

        ApplicationCommand = 2,

        MessageComponent = 3,

        ApplicationCommandAutoComplete = 4,

        ModalSubmit = 5
    }
}
