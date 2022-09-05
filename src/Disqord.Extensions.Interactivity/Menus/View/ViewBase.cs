using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Represents a menu view.
/// </summary>
public abstract partial class ViewBase : IAsyncDisposable
{
    private const int MaxRowWidth = 5;

    /// <summary>
    ///     Gets the menu of this view.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this view is not currently attached to a menu.
    /// </exception>
    public MenuBase Menu
    {
        get
        {
            if (_menu == null)
                Throw.InvalidOperationException("This property must not be accessed before the view is attached to a menu.");

            return _menu;
        }
        internal set => _menu = value;
    }
    internal MenuBase? _menu;

    /// <summary>
    ///     Gets whether this view has changes.
    /// </summary>
    public bool HasChanges { get; internal set; }

    /// <summary>
    ///     Gets or sets the message template of this view.
    /// </summary>
    public Action<LocalMessageBase>? MessageTemplate
    {
        get => _messageTemplate;
        set
        {
            lock (this)
            {
                ReportChanges();

                _messageTemplate = value;
            }
        }
    }

    private readonly List<ViewComponent>[] _rows;
    private readonly ISynchronizedDictionary<string, InteractableViewComponent> _interactables;
    private Action<LocalMessageBase>? _messageTemplate;

    /// <summary>
    ///     Instantiates a new <see cref="ViewBase"/> with the specified message template.
    /// </summary>
    /// <param name="messageTemplate"> The message template for this view. </param>
    protected ViewBase(Action<LocalMessageBase>? messageTemplate)
    {
        _messageTemplate = messageTemplate;
        _rows = new List<ViewComponent>[5];
        for (var i = 0; i < 5; i++)
            _rows[i] = new List<ViewComponent>(5);

        _interactables = new SynchronizedDictionary<string, InteractableViewComponent>();

        foreach (var component in ReflectComponents(this))
            AddComponent(component);
    }

    /// <summary>
    ///     Reports that this view has changes.
    ///     This is called automatically when changing this view's components or the components' properties.
    /// </summary>
    protected void ReportChanges()
    {
        if (_menu != null)
            HasChanges = true;
    }

    /// <summary>
    ///     By default called once on initialization and on handling interactions if the view has changes.
    /// </summary>
    /// <returns></returns>
    public virtual ValueTask UpdateAsync()
    {
        return default;
    }

    /// <summary>
    ///     Gets a unique custom ID for the given interactable component.
    ///     Called by <see cref="AddComponent"/>.
    ///     Defaults to <see cref="Guid.NewGuid"/>.
    ///     This can be used to generate custom IDs for views that persist between bot restarts.
    /// </summary>
    /// <param name="component"> The component to get the custom ID for. </param>
    /// <returns>
    ///     A unique custom ID for the component.
    /// </returns>
    protected internal virtual string GetCustomId(InteractableViewComponent component)
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    ///     Adds a component to this view.
    /// </summary>
    /// <param name="component"> The component to add. </param>
    /// <exception cref="ArgumentException"> The component belonged to another view. </exception>
    /// <exception cref="ArgumentException"> A component with this custom ID had already been added. </exception>
    /// <exception cref="ArgumentException"> No free row was available for the component. </exception>
    /// <exception cref="ArgumentException"> The component could not be added on a row as the total width of the row must be 5 or less. </exception>
    public void AddComponent(ViewComponent component)
    {
        lock (this)
        {
            if (component.View != null && component.View != this)
                throw new ArgumentException("The component belongs to another view.", nameof(component));

            if (component is InteractableViewComponent interactableComponent)
            {
                var customId = GetCustomId(interactableComponent);
                if (!_interactables.TryAdd(customId, interactableComponent))
                    throw new ArgumentException($"A component with the custom ID '{customId}' has already been added.", nameof(component));

                interactableComponent.CustomId = customId;
            }

            static bool TryAddComponent(List<ViewComponent> row, ViewComponent component)
            {
                var totalWidth = row.Sum(x => x.Width);
                if (totalWidth + component.Width > MaxRowWidth)
                    return false;

                if (component.Position == null || component.Position.Value > row.Count - 1)
                {
                    row.Add(component);
                }
                else
                {
                    row.Insert(component.Position.Value, component);
                }

                return true;
            }

            if (component.Row == null)
            {
                static void FindFreeRow(List<ViewComponent>[] rows, ViewComponent component)
                {
                    foreach (var row in rows)
                    {
                        if (TryAddComponent(row, component))
                            return;
                    }

                    throw new ArgumentException("No free row available for the component.", nameof(component));
                }

                FindFreeRow(_rows, component);
            }
            else
            {
                var row = _rows[component.Row.Value];
                if (!TryAddComponent(row, component))
                    throw new ArgumentException($"The component cannot be added on row {component.Row} as the total width of the row must be 5 or less.", nameof(component));
            }

            component.View = this;
            ReportChanges();
        }
    }

    /// <summary>
    ///     Removes a component from this view.
    /// </summary>
    /// <param name="component"> The component to remove. </param>
    /// <returns>
    ///     <see langword="true"/> if the component was removed.
    /// </returns>
    public bool RemoveComponent(ViewComponent component)
    {
        lock (this)
        {
            foreach (var row in _rows)
            {
                if (!row.Remove(component))
                    continue;

                if (component is InteractableViewComponent interactableComponent)
                    _interactables.Remove(interactableComponent.CustomId);

                component.View = null;
                ReportChanges();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Removes all components from this view.
    /// </summary>
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

    /// <summary>
    ///     Enumerates all components of this view.
    /// </summary>
    /// <returns>
    ///     An enumerable of this view's components.
    /// </returns>
    public IEnumerable<ViewComponent> EnumerateComponents()
    {
        lock (this)
        {
            return _rows.SelectMany(x => x).ToArray();
        }
    }

    /// <summary>
    ///     Executes this view, i.e. calls the matching interactable component's callback.
    /// </summary>
    /// <param name="e"> The event data. </param>
    /// <param name="task"> The <see cref="ValueTask"/> representing the callback. </param>
    /// <returns>
    ///     <see langword="true"/> if a component matched the event data.
    /// </returns>
    public bool TryExecuteComponent(InteractionReceivedEventArgs e, out ValueTask task)
    {
        if (_interactables.TryGetValue((e.Interaction as IComponentInteraction)!.CustomId, out var component))
        {
            task = component.ExecuteAsync(e);
            return true;
        }

        task = default;
        return false;
    }

    /// <summary>
    ///     Converts this view into a <see cref="LocalMessageBase"/> representation of it.
    /// </summary>
    public virtual void FormatLocalMessage(LocalMessageBase message)
    {
        lock (this)
        {
            _messageTemplate?.Invoke(message);

            if (message.AllowedMentions.GetValueOrDefault() == null)
                message.AllowedMentions = LocalAllowedMentions.None;

            var components = new List<LocalRowComponent>();
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

                components.Add(localRowComponent);
            }

            message.Components = components;
        }
    }

    /// <summary>
    ///     Disposes of this view.
    /// </summary>
    /// <remarks>
    ///     By default does nothing.
    /// </remarks>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    public virtual ValueTask DisposeAsync()
    {
        return default;
    }
}
