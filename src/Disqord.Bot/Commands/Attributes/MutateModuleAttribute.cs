using System;

namespace Disqord.Bot
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MutateModuleAttribute : Attribute
    {
        public MutateModuleAttribute()
        { }
    }
}
