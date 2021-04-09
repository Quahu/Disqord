using System.Threading.Tasks;
using Disqord.Utilities.Binding;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents a callback handled by the <see cref="ICommandQueue"/>
    ///     that executes commands using the specified input and context.
    /// </summary>
    /// <param name="input"> The input possibly containing commands. </param>
    /// <param name="context"> The command context. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the command execution.
    /// </returns>
    public delegate Task CommandQueueDelegate(string input, DiscordCommandContext context);

    /// <summary>
    ///     Represents a type responsible for handling command execution load.
    /// </summary>
    public interface ICommandQueue : IBindable<DiscordBotBase>
    {
        /// <summary>
        ///     Gets the bot this queue is bound to.
        /// </summary>
        DiscordBotBase Bot { get; }

        /// <summary>
        ///     Schedules execution of commands on this queue.
        /// </summary>
        /// <param name="input"> The input possibly containing commands. </param>
        /// <param name="context"> The command context. </param>
        /// <param name="func"> The execution callback the queue should call. </param>
        void Post(string input, DiscordCommandContext context, CommandQueueDelegate func);
    }
}
