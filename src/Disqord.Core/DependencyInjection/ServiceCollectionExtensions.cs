using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Qommon;

namespace Disqord.DependencyInjection.Extensions;

internal static class ServiceCollectionExtensions
{
    public static Type? GetImplementationType(this ServiceDescriptor descriptor)
        => descriptor.ImplementationType ?? (descriptor.ImplementationInstance?.GetType() ?? descriptor.ImplementationFactory?.GetType().GenericTypeArguments[1]);

    public static bool TryAdd<TService, TImplementation>(this IServiceCollection collection, ServiceLifetime lifetime)
    {
        Guard.IsNotNull(collection);

        return collection.TryAdd(ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), lifetime));
    }

    public static bool TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(descriptor);

        if (!collection.Any(d => d.ServiceType == descriptor.ServiceType))
        {
            collection.Add(descriptor);
            return true;
        }

        return false;
    }

    public static bool TryAddSingleton(this IServiceCollection collection, Type service)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);

        var descriptor = ServiceDescriptor.Singleton(service, service);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationType);

        var descriptor = ServiceDescriptor.Singleton(service, implementationType);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationFactory);

        var descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddSingleton<TService>(this IServiceCollection collection)
        where TService : class
    {
        Guard.IsNotNull(collection);

        return TryAddSingleton(collection, typeof(TService), typeof(TService));
    }

    public static bool TryAddSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        Guard.IsNotNull(collection);

        return TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
    }

    public static bool TryAddSingleton<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(instance);

        var descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
        => services.TryAdd(ServiceDescriptor.Singleton(implementationFactory));

    public static bool TryAddScoped(this IServiceCollection collection, Type service)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);

        var descriptor = ServiceDescriptor.Scoped(service, service);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddScoped(this IServiceCollection collection, Type service, Type implementationType)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationType);

        var descriptor = ServiceDescriptor.Scoped(service, implementationType);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationFactory);

        var descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddScoped<TService>(this IServiceCollection collection)
        where TService : class
    {
        Guard.IsNotNull(collection);

        return TryAddScoped(collection, typeof(TService), typeof(TService));
    }

    public static bool TryAddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        Guard.IsNotNull(collection);

        return TryAddScoped(collection, typeof(TService), typeof(TImplementation));
    }

    public static bool TryAddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
        => services.TryAdd(ServiceDescriptor.Scoped(implementationFactory));

    public static bool TryAddTransient(this IServiceCollection collection, Type service)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);

        var descriptor = ServiceDescriptor.Transient(service, service);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddTransient(this IServiceCollection collection, Type service, Type implementationType)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationType);

        var descriptor = ServiceDescriptor.Transient(service, implementationType);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(service);
        Guard.IsNotNull(implementationFactory);

        var descriptor = ServiceDescriptor.Transient(service, implementationFactory);
        return TryAdd(collection, descriptor);
    }

    public static bool TryAddTransient<TService>(this IServiceCollection collection)
        where TService : class
    {
        Guard.IsNotNull(collection);

        return TryAddTransient(collection, typeof(TService), typeof(TService));
    }

    public static bool TryAddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        Guard.IsNotNull(collection);

        return TryAddTransient(collection, typeof(TService), typeof(TImplementation));
    }

    public static bool TryAddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
        => services.TryAdd(ServiceDescriptor.Transient(implementationFactory));

    public static bool TryAddSingletonEnumerable<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.TryAddEnumerable(ServiceDescriptor.Singleton<TService, TImplementation>());

    public static bool TryAddEnumerable(this IServiceCollection services, ServiceDescriptor descriptor)
    {
        Guard.IsNotNull(services);
        Guard.IsNotNull(descriptor);

        var implementationType = descriptor.GetImplementationType();
        if (implementationType == typeof(object) || implementationType == descriptor.ServiceType)
            throw new ArgumentException("Invalid descriptor implementation type.", nameof(descriptor));

        var count = services.Count;
        for (var i = 0; i < count; i++)
        {
            var service = services[i];
            if (service.ServiceType == descriptor.ServiceType && service.GetImplementationType() == implementationType)
                return false;
        }

        services.Add(descriptor);
        return true;
    }

    public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        Guard.IsNotNull(collection);
        Guard.IsNotNull(descriptor);

        var count = collection.Count;
        for (var i = 0; i < count; i++)
        {
            if (collection[i].ServiceType == descriptor.ServiceType)
            {
                collection.RemoveAt(i);
                break;
            }
        }

        collection.Add(descriptor);
        return collection;
    }

    public static bool Remove<TService>(this IServiceCollection collection)
    {
        Guard.IsNotNull(collection);

        var count = collection.Count;
        for (var i = 0; i < count; i++)
        {
            if (collection[i].ServiceType == typeof(TService))
            {
                collection.RemoveAt(i);
                return true;
            }
        }

        return false;
    }
}