using System.Collections.Generic;

namespace Disqord
{
    public interface ISlashCommand : IApplicationCommand
    {
        string Description { get; }

        IReadOnlyList<ISlashCommandOption> Options { get; }
    }
}
