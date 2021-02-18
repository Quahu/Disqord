using System;

namespace Disqord
{
    public sealed class EntityNotManagedException : Exception
    {
        public EntityNotManagedException()
            : base("This entity is not managed by a client.")
        { }
    }
}
