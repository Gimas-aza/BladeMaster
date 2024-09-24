using System;
using Assets.EntryPoint;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.DataManagement
{
    public class DataStorageSystem : ILoadSystem, ISaveSystem
    {
        private readonly IStorage _storage;

        public DataStorageSystem(IStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage), "Storage cannot be null.");
        }

        public async UniTask<DataStorage> LoadAsync()
        {
            try
            {
                return await _storage.LoadAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data: {ex.Message}");
                throw;
            }
        }

        public void SaveAsync()
        {
            try
            {
                _storage.SaveAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }
    }
}