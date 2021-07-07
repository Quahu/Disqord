using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <inheritdoc/>
    /// <summary>
    ///     <inheritdoc/>
    ///     Overrides <see cref="MenuBase.CheckInteractionAsync"/> restricting the menu usage to a specific user.
    /// </summary>
    public abstract class AuthorMenuBase : MenuBase
    {
        /// <summary>
        ///     Gets the ID of the user this menu is restricted to.
        /// </summary>
        public Snowflake AuthorId { get; protected set; }

        /// <inheritdoc/>
        /// <param name="authorId"> The ID of the user this menu is restricted to. </param>
        protected AuthorMenuBase(Snowflake authorId, ViewBase view)
            : base(view)
        {
            AuthorId = authorId;
        }

        /// <inheritdoc/>
        /// <summary>
        ///     Checks if the ID of the user who reacted is equal to <see cref="AuthorId"/>.
        /// </summary>
        protected override ValueTask<bool> CheckInteractionAsync(InteractionReceivedEventArgs e)
            => new(e.AuthorId == AuthorId);
    }
}
