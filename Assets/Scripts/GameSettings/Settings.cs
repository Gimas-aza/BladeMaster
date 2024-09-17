using System;
using Assets.EntryPoint;
using Assets.MVP.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSettings
{
    public class Settings : IModel, IInitializer
    {
        private int _qualityIndex;
        private ISaveSystem _saveSystem;
        private ISettingsData _settingsData;

        public void Init(ISaveSystem saveSystem, ISettingsData settingsData)
        {
            _saveSystem = saveSystem;
            _settingsData = settingsData;
            _qualityIndex = settingsData.QualityIndex;

            ChangeQuality(_qualityIndex);
        }

        public void SubscribeToEvents(ref UnityAction<int> changeQuality, ref Func<int> currentQuality)
        {
            changeQuality += ChangeQuality;
            currentQuality += () => _qualityIndex;
        }

        private void ChangeQuality(int index)
        {
            _qualityIndex = index;
            _settingsData.QualityIndex = index;
            QualitySettings.SetQualityLevel(index);
            _saveSystem.SaveAsync();
        }
    }
}
