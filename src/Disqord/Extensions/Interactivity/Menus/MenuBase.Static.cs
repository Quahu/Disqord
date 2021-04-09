using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;

namespace Disqord.Extensions.Interactivity.Menus
{
    public abstract partial class MenuBase
    {
        private static readonly ISynchronizedDictionary<Type, KeyValuePair<IEmoji, MethodInfo>[]> _typeCache;

        static MenuBase()
        {
            _typeCache = new SynchronizedDictionary<Type, KeyValuePair<IEmoji, MethodInfo>[]>();
        }

        static ISynchronizedDictionary<IEmoji, Button> GetButtons(MenuBase menu)
        {
            var methods = _typeCache.GetOrAdd(menu.GetType(), static x =>
            {
                var methods = x.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                IDictionary<IEmoji, MethodInfo> buttons = null;
                for (var i = 0; i < methods.Length; i++)
                {
                    var method = methods[i];
                    var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                    if (buttonAttribute == null)
                        continue;

                    if (method.ContainsGenericParameters)
                        throw new InvalidOperationException("A button callback must not contain generic parameters.");

                    if (method.ReturnType != typeof(Task))
                        throw new InvalidOperationException("A button callback must return a non-generic Task.");

                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(ButtonEventArgs))
                        throw new InvalidOperationException("A button callback must contain a single ButtonEventArgs parameter.");

                    if (buttons == null)
                        buttons = new Dictionary<IEmoji, MethodInfo>();

                    buttons.Add(buttonAttribute.Emoji, method);
                }

                KeyValuePair<IEmoji, MethodInfo>[] array;
                if (buttons != null)
                {
                    array = new KeyValuePair<IEmoji, MethodInfo>[buttons.Count];
                    buttons.CopyTo(array, 0);
                }
                else
                {
                    array = Array.Empty<KeyValuePair<IEmoji, MethodInfo>>();
                }

                return array;
            });

            var buttons = new SynchronizedDictionary<IEmoji, Button>(methods.Length);
            for (var i = 0; i < methods.Length; i++)
            {
                var (emoji, method) = methods[i];
                var button = ButtonFactory(emoji, method, i, menu);
                buttons.Add(button.Emoji, button);
            }

            return buttons;
        }

        static Button ButtonFactory(IEmoji emoji, MethodInfo method, int position, object instance)
        {
            ButtonCallback buttonCallback;
            try
            {
                buttonCallback = method.CreateDelegate<ButtonCallback>(instance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                "Failed to create a button callback delegate. Methods marked with the ButtonAttribute must match the ButtonCallback delegate's signature.",
                ex);
            }

            return new Button(emoji, buttonCallback, position);
        }
    }
}
