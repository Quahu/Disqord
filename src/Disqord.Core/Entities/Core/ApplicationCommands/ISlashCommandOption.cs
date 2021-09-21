using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface ISlashCommandOption : INamable, IJsonUpdatable<ApplicationCommandOptionJsonModel>
    {
        SlashCommandOptionType Type { get; }

        string Description { get; }

        bool IsRequired { get; }

        IReadOnlyList<ISlashCommandOptionChoice> Choices { get; }

        IReadOnlyList<ISlashCommandOption> Options { get; }

        IReadOnlyList<ChannelType> ChannelTypes { get; }
    }
}
