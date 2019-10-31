namespace Disqord.Events
{
    public sealed class UserNoteUpdatedEventArgs : DiscordEventArgs
    {
        public Snowflake UserId { get; }

        public string OldNote { get; }

        public string NewNote { get; }

        internal UserNoteUpdatedEventArgs(DiscordClientBase client,
            Snowflake userId,
            string oldNote,
            string newNote) : base(client)
        {
            UserId = userId;
            OldNote = oldNote;
            NewNote = newNote;
        }
    }
}
