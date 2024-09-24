using System;
using System.Collections.Generic;
using Assets.EntryPoint;
using UnityEngine;

namespace Assets.DI
{
    public class DIContainer : IDIContainer
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIRegistration> _registrations = new();
        private readonly HashSet<(string, Type)> _resolutions = new();

        public DIContainer(DIContainer parent = null)
        {
            _parentContainer = parent;
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(null, factory);
        }

        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, true);
        }

        public void RegisterTransient<T>(Func<DIContainer, T> factory)
        {
            RegisterTransient(null, factory);
        }

        public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }

        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof(T));

            if (_registrations.ContainsKey(key))
                Debug.LogError(
                    $"DI: Already registered with tag {key.Item1} and type {key.Item2.FullName}"
                );

            _registrations[key] = new DIRegistration 
            {
                IsSingleton = true,
                Instance = instance
            };
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resolutions.Contains(key))
            {
                Debug.LogError(
                    $"DI: Cycle dependency for tag {key.Item1} and type {key.Item2.FullName}"
                );
                return default;
            }

            _resolutions.Add(key);

            try
            {
                if (_registrations.TryGetValue(key, out var registration))
                {
                    if (registration.IsSingleton)
                    {
                        if (registration.Instance == null && registration.Factory != null)
                            registration.Instance = registration.Factory(this);
                        return (T)registration.Instance;
                    }

                    return (T)registration.Factory(this);
                }

                if (_parentContainer != null)
                    return _parentContainer.Resolve<T>(tag);
            }
            finally
            {
                _resolutions.Remove(key);
            }

            Debug.LogError(
                $"DI: No registration found with tag {key.Item1} and type {key.Item2.FullName}"
            );
            return default;
        }

        private void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
        {
            if (_registrations.ContainsKey(key))
                Debug.LogError(
                    $"DI: Already registered with tag {key.Item1} and type {key.Item2.FullName}"
                );

            _registrations[key] = new DIRegistration
            {
                Factory = (container) => factory(container),
                IsSingleton = isSingleton
            };
        }
    }
}
