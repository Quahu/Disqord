using System.ComponentModel;
using System.Threading;

namespace Disqord.Utilities.Threading
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BetterSemaphoreSlim : SemaphoreSlim
    {
        public int MaximumCount { get; }

        public BetterSemaphoreSlim()
            : this(1, 1)
        { }

        public BetterSemaphoreSlim(int initialCount)
            : this(initialCount, int.MaxValue)
        { }

        public BetterSemaphoreSlim(int initialCount, int maxCount)
            : base(initialCount, maxCount)
        {
            MaximumCount = maxCount;
        }
    }
}
