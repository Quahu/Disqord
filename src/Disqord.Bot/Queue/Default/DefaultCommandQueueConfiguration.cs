using System.ComponentModel.DataAnnotations;

namespace Disqord.Bot
{
    public class DefaultCommandQueueConfiguration
    {
        /// <summary>
        ///     Gets or sets the degree of parallelism for the queue,
        ///     i.e. the amount of parallel command executions per-bucket.
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int DegreeOfParallelism { get; set; } = 5;
    }
}