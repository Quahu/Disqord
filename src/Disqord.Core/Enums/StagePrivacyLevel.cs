namespace Disqord
{
    /// <summary>
    ///     Represents the privacy level of a stage.
    /// </summary>
    public enum StagePrivacyLevel
    {
        /// <summary>
        ///     The stage is visible publicly on stage discovery.
        /// </summary>
        Public = 1,

        /// <summary>
        ///     The stage is visible to members only.
        /// </summary>
        GuildOnly = 2
    }
}
