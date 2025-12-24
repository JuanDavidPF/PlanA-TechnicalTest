using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA.Architecture.Services
{
    public static class ServiceLocator
    {
        static readonly private Dictionary<Type, IGameService> _services = new();

        public static T Register<T>(Type interfaceType, T serviceInstance) where T : IGameService
        {
            if (_services.TryAdd(interfaceType, serviceInstance))
            {
                serviceInstance.Initialize();
                return serviceInstance;
            }

            Debug.LogWarning($"{interfaceType} has already been registered");
            return default;
        }

        public static T Get<T>() where T : IGameService
        {
            if (_services != null && _services.TryGetValue(typeof(T), out IGameService serviceInstance))
            {
                return (T)serviceInstance;
            }

            Debug.LogError($"ServiceLocator: Could not find service of type {typeof(T)}");
            return default;
        }

        public static bool TryGet<T>(out T result) where T : IGameService
        {
            result = default;
            if (_services == null || !_services.TryGetValue(typeof(T), out IGameService serviceInstance)) return false;
            result = (T)serviceInstance;
            return true;
        }

        public static void CleanUp()
        {
            foreach (IGameService service in _services.Values)
            {
                service.DeInitialize();
            }

            _services.Clear();
        }
    }
}