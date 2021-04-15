using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.DependencyInjection.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static Type GetImplementationType(this ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationType != null)
            {
                return descriptor.ImplementationType;
            }
            else if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance.GetType();
            }
            else if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory.GetType().GenericTypeArguments[1];
            }

            return null;
        }

        public static bool TryAdd<TService, TImplementation>(this IServiceCollection collection, ServiceLifetime lifetime)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.TryAdd(ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), lifetime));
        }

        public static bool TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            if (!collection.Any(d => d.ServiceType == descriptor.ServiceType))
            {
                collection.Add(descriptor);
                return true;
            }

            return false;
        }

        public static bool TryAddSingleton(this IServiceCollection collection, Type service)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var descriptor = ServiceDescriptor.Singleton(service, service);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            var descriptor = ServiceDescriptor.Singleton(service, implementationType);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));

            var descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddSingleton<TService>(this IServiceCollection collection)
            where TService : class
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return TryAddSingleton(collection, typeof(TService), typeof(TService));
        }

        public static bool TryAddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
        }

        public static bool TryAddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
            => services.TryAdd(ServiceDescriptor.Singleton(implementationFactory));

        public static bool TryAddScoped(this IServiceCollection collection, Type service)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var descriptor = ServiceDescriptor.Singleton(service, service);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddScoped(this IServiceCollection collection, Type service, Type implementationType)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            var descriptor = ServiceDescriptor.Scoped(service, implementationType);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));

            var descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
            return TryAdd(collection, descriptor);
        }

        public static bool TryAddScoped<TService>(this IServiceCollection collection)
            where TService : class
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return TryAddScoped(collection, typeof(TService), typeof(TService));
        }

        public static bool TryAddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return TryAddScoped(collection, typeof(TService), typeof(TImplementation));
        }

        public static bool TryAddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
            => services.TryAdd(ServiceDescriptor.Scoped(implementationFactory));

        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            for (var i = 0; i < collection.Count; i++)
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
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            for (var i = 0; i < collection.Count; i++)
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
}
