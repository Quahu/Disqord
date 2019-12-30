using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Events;
using Qommon.Collections;

namespace Disqord.Interactivity.Pagination
{
    public sealed partial class StaticPaginator : PaginatorBase
    {
        public static PaginatorTimeoutBehavior DefaultTimeoutBehavior = PaginatorTimeoutBehavior.DeleteMessage;

        public IReadOnlyDictionary<IEmoji, Page> Pages => new ReadOnlyDictionary<IEmoji, Page>(_pages);

        public override Page DefaultPage { get; }

        public PaginatorTimeoutBehavior TimeoutBehavior { get; }

        private readonly Dictionary<IEmoji, Page> _pages;

        public StaticPaginator(IReadOnlyDictionary<IEmoji, Page> pages, Page defaultPage)
            : this(pages, defaultPage, DefaultTimeoutBehavior)
        { }

        public StaticPaginator(IReadOnlyDictionary<IEmoji, Page> pages, Page defaultPage, PaginatorTimeoutBehavior timeoutBehavior)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            if (defaultPage == null)
                throw new ArgumentNullException(nameof(defaultPage));

            if (!Enum.IsDefined(typeof(PaginatorTimeoutBehavior), timeoutBehavior))
                throw new ArgumentOutOfRangeException(nameof(timeoutBehavior));

            _pages = pages.ToDictionary(x => x.Key, x => x.Value);
            DefaultPage = defaultPage;
            TimeoutBehavior = timeoutBehavior;
        }

        protected internal override async ValueTask InitialiseAsync()
        {
            foreach (var emoji in _pages.Keys)
                await Message.AddReactionAsync(emoji).ConfigureAwait(false);
        }

        protected internal override ValueTask ExpiredAsync()
        {
            if (TimeoutBehavior == PaginatorTimeoutBehavior.None)
                return default;

            var task = TimeoutBehavior switch
            {
                PaginatorTimeoutBehavior.DeleteMessage => Message.DeleteAsync(),
                PaginatorTimeoutBehavior.ClearReactions => Message.ClearReactionsAsync(),
                _ => throw new InvalidOperationException()
            };

            return new ValueTask(task);
        }

        protected internal override async ValueTask<Page> GetPageAsync(ReactionAddedEventArgs e)
        {
            if (e.User.Id == Channel.Client.CurrentUser.Id)
                return default;

            var user = e.User.HasValue
                ? (IUser) e.User.Value
                : await e.User.Downloadable.DownloadAsync().ConfigureAwait(false);
            if (user.IsBot)
                return default;

            return _pages.GetValueOrDefault(e.Emoji);
        }
    }
}
