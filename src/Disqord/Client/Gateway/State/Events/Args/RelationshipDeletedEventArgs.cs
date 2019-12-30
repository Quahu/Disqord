namespace Disqord.Events
{
    public sealed class RelationshipDeletedEventArgs : DiscordEventArgs
    {
        public CachedRelationship Relationship { get; }

        internal RelationshipDeletedEventArgs(CachedRelationship relationship) : base(relationship.Client)
        {
            Relationship = relationship;
        }
    }
}
