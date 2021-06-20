using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    public abstract partial class ViewBase
    {
        public MenuBase Menu { get; internal set; }

        public bool HasChanges { get; internal set; }

        public LocalMessage TemplateMessage
        {
            get => _templateMessage;
            set
            {
                ReportChanges();
                _templateMessage = value;
            }
        }

        private readonly List<ViewComponent>[] _rows;
        private readonly Dictionary<string, InteractableViewComponent> _interactables;
        private LocalMessage _templateMessage;

        protected ViewBase(LocalMessage templateMessage)
            : this()
        {
            _templateMessage = templateMessage;
        }

        protected ViewBase()
        {
            _rows = new List<ViewComponent>[5];
            for (var i = 0; i < 5; i++)
                _rows[i] = new List<ViewComponent>(5);

            _interactables = new Dictionary<string, InteractableViewComponent>();

            foreach (var component in ReflectComponents(this))
                AddComponent(component);
        }

        protected void ReportChanges()
        {
            if (Menu != null)
                HasChanges = true;
        }

        public virtual ValueTask UpdateAsync()
            => default;

        public void AddComponent(ViewComponent component)
        {
            lock (this)
            {
                if (component.View != null && component.View != this)
                    throw new ArgumentException("The component belongs to a different view.", nameof(component));

                if (component is InteractableViewComponent interactableComponent)
                {
                    if (!_interactables.TryAdd(interactableComponent.CustomId, interactableComponent))
                        throw new ArgumentException("The component has a duplicate custom ID.", nameof(component));
                }

                static bool TryFitComponent(List<ViewComponent> row, ViewComponent component)
                {
                    var totalWidth = row.Sum(x => x.Width);
                    if (totalWidth + component.Width > 5)
                        return false;

                    if (component.Position == -1)
                        row.Add(component);
                    else
                        row.Insert(component.Position, component);

                    return true;
                }

                if (component.Row == -1)
                {
                    static void FindFreeRow(List<ViewComponent>[] rows, ViewComponent component)
                    {
                        foreach (var row in rows)
                        {
                            if (TryFitComponent(row, component))
                                return;
                        }

                        throw new ArgumentException("No free row found for the component.", nameof(component));
                    }

                    FindFreeRow(_rows, component);
                }
                else
                {
                    var row = _rows[component.Row];
                    if (!TryFitComponent(row, component))
                        throw new ArgumentException($"The component cannot fit on row {component.Row} as the total width must be 5 or less.", nameof(component));
                }

                component.View = this;
                ReportChanges();
            }
        }

        public void RemoveComponent(ViewComponent component)
        {
            lock (this)
            {
                foreach (var row in _rows)
                {
                    if (row.Remove(component))
                    {
                        if (component is InteractableViewComponent interactableComponent)
                            _interactables.Remove(interactableComponent.CustomId);

                        component.View = null;
                        ReportChanges();
                        return;
                    }
                }
            }
        }

        public void ClearComponents()
        {
            lock (this)
            {
                foreach (var row in _rows)
                    row.Clear();

                _interactables.Clear();
                ReportChanges();
            }

        }

        public IEnumerable<ViewComponent> EnumerateComponents()
        {
            lock (this)
            {
                return _rows.SelectMany(x => x).ToArray();
            }
        }

        public ValueTask ExecuteAsync(InteractionReceivedEventArgs e)
        {
            lock (this)
            {
                if (_interactables.TryGetValue((e.Interaction as IComponentInteraction).ComponentId, out var component))
                    return component.ExecuteAsync(e);

                return default;
            }
        }

        public virtual LocalMessage ToLocalMessage()
        {
            lock (this)
            {
                var localMessage = (TemplateMessage?.Clone() ?? new LocalMessage())
                    .WithAllowedMentions(LocalAllowedMentions.None);
                foreach (var row in _rows)
                {
                    if (row.Count == 0)
                        continue;

                    var localRowComponent = new LocalRowComponent();
                    foreach (var component in row)
                    {
                        var localComponent = component.ToLocalComponent();
                        localRowComponent.AddComponent(localComponent);
                    }

                    localMessage.AddComponent(localRowComponent);
                }

                return localMessage;
            }
        }
    }
}
