using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio.Core.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> Services = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDomainReload()
        {
            Services.Clear();
        }

        public static void Register<T>(T service) where T : class, IService
        {
            var type = typeof(T);
            if (Services.ContainsKey(type))
            {
                throw new ServiceAlreadyRegisteredException(type);
            }

            Services[type] = service;
        }

        public static void Register<TInterface, TImplementation>(TImplementation service)
            where TInterface : class, IService
            where TImplementation : class, TInterface
        {
            var type = typeof(TInterface);
            if (Services.ContainsKey(type))
            {
                throw new ServiceAlreadyRegisteredException(type);
            }

            Services[type] = service;
        }

        public static bool TryGet<T>(out T service) where T : class, IService
        {
            if (Services.TryGetValue(typeof(T), out var instance))
            {
                service = instance as T;
                return service != null;
            }

            service = null;
            return false;
        }

        public static T Get<T>() where T : class, IService
        {
            if (TryGet<T>(out var service))
            {
                return service;
            }

            throw new InvalidOperationException($"Service of type '{typeof(T).Name}' is not registered.");
        }

        public static bool IsRegistered<T>() where T : class, IService
        {
            return Services.ContainsKey(typeof(T));
        }

        public static void Unregister<T>() where T : class, IService
        {
            Services.Remove(typeof(T));
        }

        public static void Clear()
        {
            Services.Clear();
        }
    }
}
