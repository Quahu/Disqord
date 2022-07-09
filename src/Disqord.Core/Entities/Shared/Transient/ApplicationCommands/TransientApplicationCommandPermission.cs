using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IApplicationCommandPermission"/>
    public class TransientApplicationCommandPermission : TransientEntity<ApplicationCommandPermissionsJsonModel>, IApplicationCommandPermission
    {
        /// <inheritdoc/>
        public Snowflake TargetId => Model.Id;

        /// <inheritdoc/>
        public ApplicationCommandPermissionType Type => Model.Type;

        /// <inheritdoc/>
        public bool HasPermission => Model.Permission;

        public TransientApplicationCommandPermission(ApplicationCommandPermissionsJsonModel model)
            : base(model)
        { }
    }
}
