using System;
using System.Threading.Tasks;

namespace Disqord.Events
{
    public abstract class AsynchronousEvent
    {
        protected AsynchronousEvent()
        { }

        protected internal abstract ValueTask InvokeAsync(object sender, EventArgs e);
    }
}
