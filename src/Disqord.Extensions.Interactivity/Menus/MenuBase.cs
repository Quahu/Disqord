using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Extensions.Interactivity.Pagination;
using Disqord.Logging;

namespace Disqord.Extensions.Interactivity.Menus
{
    public abstract class MenuBase : IAsyncDisposable
    {
        public InteractivityExtension Interactivity { get; internal set; }

        public DiscordClientBase Client => Interactivity.Client;

        public IMessageChannel Channel { get; internal set; }

        public IUserMessage Message { get; internal set; }

        public bool AddsReactions { get; }

        public bool TriggersOnRemoval { get; }

        internal MenuWrapper _wrapper;

        private bool _isRunning;

        private readonly LockedDictionary<IEmoji, Button> _buttons = new LockedDictionary<IEmoji, Button>();

        private static readonly LockedDictionary<Type, (IEmoji, MethodInfo, ButtonFactory)[]> _typeCache
            = new LockedDictionary<Type, (IEmoji, MethodInfo, ButtonFactory)[]>();

        private delegate Button ButtonFactory(IEmoji emoji, MethodInfo method, int position, object instance);

        public MenuBase(bool addReactions = true, bool triggerOnRemoval = true)
        {
            AddsReactions = addReactions;
            TriggersOnRemoval = triggerOnRemoval;
            var buttons = _typeCache.GetOrAdd(GetType(), x =>
            {
                var methods = x.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                List<(IEmoji emoji, MethodInfo method, ButtonFactory factory)> buttons = null;
                for (var i = 0; i < methods.Length; i++)
                {
                    var method = methods[i];
                    var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                    if (buttonAttribute == null)
                        continue;

                    if (method.ContainsGenericParameters)
                        throw new InvalidOperationException("A button callback must not contain generic parameters.");

                    if (method.ReturnType != typeof(Task))
                        throw new InvalidOperationException("A button callback must return Task.");

                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(ButtonEventArgs))
                        throw new InvalidOperationException("A button callback must contain a single ButtonEventArgs parameter.");

                    static Button ButtonFactory(IEmoji emoji, MethodInfo method, int position, object instance)
                    {
                        ButtonCallback buttonCallback;
                        try
                        {
                            buttonCallback = (ButtonCallback) method.CreateDelegate(typeof(ButtonCallback), instance);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Failed to create a button callback delegate. Methods marked with the ButtonAttribute must match the ButtonCallback delegate's signature.",
                                ex);
                        }

                        return new Button(emoji, buttonCallback, position);
                    }

                    if (buttons == null)
                        buttons = new List<(IEmoji, MethodInfo, ButtonFactory)>();

                    buttons.Add((buttonAttribute.Emoji, method, ButtonFactory));
                }

                return buttons?.ToArray() ?? Array.Empty<(IEmoji, MethodInfo, ButtonFactory)>();
            });

            for (var i = 0; i < buttons.Length; i++)
            {
                var (emoji, method, factory) = buttons[i];
                var button = factory(emoji, method, i, this);
                _buttons.Add(button.Emoji, button);
            }
        }

        internal async Task OnButtonAsync(ButtonEventArgs e)
        {
            if (!_buttons.TryGetValue(e.Emoji, out var button))
                return;

            bool check;
            try
            {
                check = await CheckReactionAsync(e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Client.Log(LogMessageSeverity.Error, "An exception occurred in a menu reaction check.", ex);
                return;
            }

            if (!check)
                return;

            if (!e.WasAdded && !TriggersOnRemoval)
                return;

            _wrapper.Update();
            try
            {
                await button.Callback(e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Client.Log(LogMessageSeverity.Error, "An exception occurred in a menu button callback.", ex);
                return;
            }
        }

        protected virtual ValueTask<bool> CheckReactionAsync(ButtonEventArgs e)
             => new ValueTask<bool>(true);

        public ValueTask AddButtonAsync(Button button)
        {
            if (button == null)
                throw new ArgumentNullException(nameof(button));

            if (_buttons.ContainsKey(button.Emoji))
                throw new ArgumentException("A button for this emoji already exists.");

            if (!_isRunning || !AddsReactions)
            {
                _buttons.Add(button.Emoji, button);
                return default;
            }
            else
            {
                return new ValueTask(InternalAddButtonAsync(button));
            }
        }

        private async Task InternalAddButtonAsync(Button button)
        {
            await Message.AddReactionAsync(button.Emoji).ConfigureAwait(false);
            _buttons.Add(button.Emoji, button);
        }

        public ValueTask RemoveButtonAsync(Button button)
            => RemoveButtonAsync(button.Emoji);

        public ValueTask RemoveButtonAsync(IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (!_buttons.TryRemove(emoji, out var button))
                throw new ArgumentException("A button for this emoji does not exist.");

            return !_isRunning || !AddsReactions
                ? default
                : new ValueTask(Message.RemoveOwnReactionAsync(button.Emoji));
        }

        public ValueTask ClearButtonsAsync()
        {
            _buttons.Clear();
            return !_isRunning
                ? default
                : Channel is CachedTextChannel textChannel && textChannel.Guild.CurrentMember.GetPermissionsFor(textChannel).ManageMessages
                    ? new ValueTask(Message.ClearReactionsAsync())
                    : default;
        }

        internal async Task StartAsync()
        {
            // TODO: wait
            if (AddsReactions)
            {
                var kvps = _buttons.ToArray();
                Array.Sort(kvps, (a, b) => a.Value.Position.CompareTo(b.Value.Position));
                foreach (var kvp in kvps)
                    await Message.AddReactionAsync(kvp.Key).ConfigureAwait(false);
            }
            _isRunning = true;
        }

        public async Task StopAsync()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            try
            {
                await _wrapper.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Client.Log(LogMessageSeverity.Error, "An exception occurred while stopping a menu.", ex);
            }
        }

        protected internal abstract Task<IUserMessage> InitialiseAsync();

        public virtual ValueTask DisposeAsync()
            => default;
    }
}
