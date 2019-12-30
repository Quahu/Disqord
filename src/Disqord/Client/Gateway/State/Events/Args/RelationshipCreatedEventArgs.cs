namespace Disqord.Events
{
    public sealed class RelationshipCreatedEventArgs : DiscordEventArgs
    {
        public CachedRelationship Relationship { get; }

        internal RelationshipCreatedEventArgs(CachedRelationship relationship) : base(relationship.Client)
        {
            Relationship = relationship;
        }
    }
}
