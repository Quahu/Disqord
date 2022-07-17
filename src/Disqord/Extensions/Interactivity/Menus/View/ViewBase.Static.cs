using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Qommon.Collections.Synchronized;

namespace Disqord.Extensions.Interactivity.Menus;

public abstract partial class ViewBase
{
    private static readonly ISynchronizedDictionary<Type, (ComponentAttribute, MemberInfo)[]> _memberCache;

    static ViewBase()
    {
        _memberCache = new SynchronizedDictionary<Type, (ComponentAttribute, MemberInfo)[]>();
    }

    private static ViewComponent[] ReflectComponents(ViewBase view)
    {
        var methods = _memberCache.GetOrAdd(view.GetType(), static x =>
        {
            IList<(ComponentAttribute, MemberInfo)>? memberCache = null;
            var members = x.GetMembers(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < members.Length; i++)
            {
                var member = members[i];
                var componentAttribute = member.GetCustomAttribute<ComponentAttribute>();
                if (componentAttribute == null)
                    continue;

                if (member is MethodInfo method)
                {
                    if (method.ContainsGenericParameters)
                        throw new InvalidOperationException("A view component callback must not contain generic parameters.");

                    if (method.ReturnType != typeof(ValueTask))
                        throw new InvalidOperationException("A view component callback must return a non-generic ValueTask.");

                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != (componentAttribute is ButtonAttribute
                        ? typeof(ButtonEventArgs)
                        : typeof(SelectionEventArgs)))
                        throw new InvalidOperationException("A view component callback must contain a single event args parameter matching the component type.");
                }
                else if (member is PropertyInfo property)
                {
                    if (!property.CanRead)
                        throw new InvalidOperationException("A view component property must be readable.");
                }
                else
                {
                    throw new InvalidOperationException("A view component must be a method or a property.");
                }

                memberCache ??= new List<(ComponentAttribute, MemberInfo)>();
                memberCache.Add((componentAttribute, member));
            }

            (ComponentAttribute, MemberInfo)[] array;
            if (memberCache != null)
            {
                array = new (ComponentAttribute, MemberInfo)[memberCache.Count];
                memberCache.CopyTo(array, 0);
            }
            else
            {
                array = Array.Empty<(ComponentAttribute, MemberInfo)>();
            }

            return array;
        });

        var memberCache = new ViewComponent[methods.Length];
        for (var i = 0; i < methods.Length; i++)
        {
            var (attribute, member) = methods[i];
            ViewComponent component = attribute switch
            {
                ButtonAttribute buttonAttribute => ButtonFactory(buttonAttribute, (member as MethodInfo)!, view),
                LinkButtonAttribute linkButtonAttribute => LinkButtonFactory(linkButtonAttribute, (member as PropertyInfo)!, view),
                SelectionAttribute selectionAttribute => SelectionFactory(selectionAttribute, (member as MethodInfo)!, view),

                // _ => ExternalFactory(attribute, member, view)
                _ => throw new InvalidOperationException($"Unknown view component attribute {attribute.GetType()}.")
            };

            component.View = view;
            memberCache[i] = component;
        }

        return memberCache;
    }

    private static ButtonViewComponent ButtonFactory(ButtonAttribute attribute, MethodInfo methodInfo, ViewBase view)
    {
        ButtonViewComponentCallback callback;
        try
        {
            callback = methodInfo.CreateDelegate<ButtonViewComponentCallback>(view);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create a {nameof(ButtonViewComponentCallback)}. "
                + $"Methods marked with the {nameof(ButtonAttribute)} must match the {nameof(callback)} delegate's signature.", ex);
        }

        return new ButtonViewComponent(attribute, callback);
    }

    private static LinkButtonViewComponent LinkButtonFactory(LinkButtonAttribute attribute, PropertyInfo propertyInfo, ViewBase view)
    {
        if (propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType != typeof(Uri))
            throw new InvalidOperationException($"A link button property must be a {nameof(String)} or a {nameof(Uri)}.");

        string url;
        try
        {
            var value = propertyInfo.GetValue(view);
            url = ((value as Uri)?.ToString() ?? value as string)!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve the URL for the link button.", ex);
        }

        return new LinkButtonViewComponent(attribute, url);
    }

    private static SelectionViewComponent SelectionFactory(SelectionAttribute attribute, MethodInfo methodInfo, ViewBase view)
    {
        SelectionViewComponentCallback callback;
        try
        {
            callback = methodInfo.CreateDelegate<SelectionViewComponentCallback>(view);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create a {nameof(SelectionViewComponentCallback)}. "
                + $"Methods marked with the {nameof(ButtonAttribute)} must match the {nameof(callback)} delegate's signature.", ex);
        }

        return new SelectionViewComponent(attribute, (methodInfo.GetCustomAttributes<SelectionOptionAttribute>() as SelectionOptionAttribute[])!, callback);
    }

    // private static ViewComponent ExternalFactory(ComponentAttribute attribute, MemberInfo memberInfo, ViewBase instance)
    // {
    //     if (attribute.GetType() != typeof(ComponentAttribute))
    //         throw new InvalidOperationException("Unsupported component attribute type.");
    //
    //     if ((memberInfo as PropertyInfo).GetValue(instance) is not LocalComponent localComponent)
    //         throw new InvalidOperationException("The value of a view component property must be a local component.");
    //
    //     return new ViewComponent(localComponent);
    // }
}