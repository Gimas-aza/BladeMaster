using System;
using Assets.DI;

namespace Assets.EntryPoint
{
    public interface IDIContainer : IResolver
    {
        void RegisterSingleton<T>(Func<DIContainer, T> factory);
        void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory);
        void RegisterTransient<T>(Func<DIContainer, T> factory);
        void RegisterTransient<T>(string tag, Func<DIContainer, T> factory);
        void RegisterInstance<T>(T instance);
        void RegisterInstance<T>(string tag, T instance);
    }
}