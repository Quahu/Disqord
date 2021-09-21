using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommand : TransientEntity<ApplicationCommandJsonModel>, IApplicationCommand
    {
        public Snowflake Id => Model.Id;

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public string Name => Model.Name;

        public ApplicationCommandType Type => Model.Type.GetValueOrDefault(ApplicationCommandType.Slash);

        public Snowflake ApplicationId => Model.ApplicationId;

        public bool IsEnabledByDefault => Model.DefaultPermission.GetValueOrDefault();

        public Snowflake Version => Model.Version;

        public TransientApplicationCommand(IClient client, ApplicationCommandJsonModel model)
            : base(client, model)
        { }

        public static IApplicationCommand Create(IClient client, ApplicationCommandJsonModel model)
            => model.Type.HasValue
                ? model.Type.Value switch
                {
                    ApplicationCommandType.Slash => new TransientSlashCommand(client, model),
                    ApplicationCommandType.User => new TransientUserContextMenuCommand(client, model),
                    ApplicationCommandType.Message => new TransientMessageContextMenuCommand(client, model),
                    _ => new TransientApplicationCommand(client, model)
                }
                : new TransientSlashCommand(client, model);  // Have to assume that this is a slash command since the type for it is often not provided
    }
}
