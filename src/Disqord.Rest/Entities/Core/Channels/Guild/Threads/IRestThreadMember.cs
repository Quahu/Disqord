namespace Disqord.Rest;

/// <inheritdoc/>
public interface IRestThreadMember : IThreadMember
{
    /// <summary>
    ///     Gets the member object of this thread member.
    /// </summary>
    /// <remarks>
    ///     <see cref="IGuildEntity.GuildId"/> of the returned member
    ///     will always be <c>0</c> as Discord does not provide it.
    /// </remarks>
    /// <returns>
    ///     The member or <see langword="null"/> if
    ///     <c>withMember</c> was not set to <see langword="true"/>.
    /// </returns>
    public IMember? Member { get; }
}
