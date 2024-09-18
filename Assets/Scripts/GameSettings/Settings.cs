using System;
using Assets.EntryPoint;
using Assets.MVP.Model;
using Assets.Sounds;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSettings
{
    public class Settings : IModel, IInitializer
    {
        private int _qualityIndex;
        private float _volume;
        private AudioComponent _audioSource;
        private ISaveSystem _saveSystem;
        private ISettingsData _settingsData;
        private UnityAction<int> _currentQuality;
        private UnityAction<float> _currentVolume;

        public void Init(ISaveSystem saveSystem, ISettingsData settingsData)
        {
            _saveSystem = saveSystem;
            _settingsData = settingsData;
            _qualityIndex = settingsData.QualityIndex;
            _volume = settingsData.Volume;

            ChangeQuality(_qualityIndex);
        }

        public void Init(AudioComponent audioSource)
        {
            _audioSource = audioSource;
        }

        public void SubscribeToEvents(
            ref UnityAction<int> changeQuality,
            ref UnityAction<int> currentQuality,
            ref UnityAction<float> changeVolume,
            ref UnityAction<float> currentVolume
        )
        {
            changeQuality += ChangeQuality;
            _currentQuality = currentQuality;
            changeVolume += ChangeVolume;
            _currentVolume = currentVolume;

            _currentVolume?.Invoke(_volume);
            _currentQuality?.Invoke(_qualityIndex);
        }

        private void ChangeQuality(int index)
        {
            _qualityIndex = index;
            _settingsData.QualityIndex = index;
            QualitySettings.SetQualityLevel(index);

            _saveSystem.SaveAsync();
        }

        private void ChangeVolume(float value)
        {
            _volume = value;
            _settingsData.Volume = value;
            if (_audioSource != null) _audioSource.Volume = value;

            _saveSystem.SaveAsync();
        }
    }
}
