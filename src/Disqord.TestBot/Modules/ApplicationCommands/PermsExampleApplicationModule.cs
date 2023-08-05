#if false
namespace Disqord.TestBot.Modules.ApplicationCommands
{
    [SlashGroup("require-permissions")]
    [RequireAuthorPermissions(Permission.ManageMessages)]
    public class PermsExampleApplicationModule : DiscordApplicationModuleBase
    {
        [SlashCommand("require-more-permissions")]
        [RequireAuthorPermissions(Permission.ManageRoles)] // This actually is ManageMessages | ManageRoles.
        public void RequireMorePermissions()               // i.e. the permissions stack until the first alias.
        { }
    }
}
#endif
