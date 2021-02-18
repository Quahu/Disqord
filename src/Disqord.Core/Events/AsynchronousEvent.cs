using System;
using System.Threading.Tasks;

namespace Disqord.Events
{
    public abstract class AsynchronousEvent
    {
        protected AsynchronousEvent()
        { }

        protected internal abstract Task InvokeAsync(object sender, EventArgs e);
    }
}