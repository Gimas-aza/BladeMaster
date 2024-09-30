using Assets.EntryPoint;
using Assets.EntryPoint.Model;
using UnityEngine;

namespace Assets.GameSettings
{
    public class Settings : IInitializer, IModel 
    {
        private int _qualityIndex;
        private float _volume;
        private IAudioSettings _audioSource;
        private ISaveSystem _saveSystem;
        private ISettingsData _settingsData;
        private IUIEvents _uiEvents;

        public void Init(IResolver container)
        {
            _saveSystem = container.Resolve<ISaveSystem>();
            _settingsData = container.Resolve<ISettingsData>();
            _audioSource = container.Resolve<IAudioSettings>();

            _qualityIndex = _settingsData.QualityIndex;
            _volume = _settingsData.Volume;

            ApplySettings();
        }

        public void SubscribeToEvents(IResolver container)
        {
            _uiEvents = container.Resolve<IUIEvents>();
            _uiEvents.UnregisterSettingsEvents();
            _uiEvents.ChangeQuality += ChangeQuality;
            _uiEvents.ChangeVolume += ChangeVolume;

            _uiEvents.CurrentQuality?.Invoke(_qualityIndex);
            _uiEvents.CurrentVolume?.Invoke(_volume);
        }

        ~Settings()
        {
            _uiEvents.UnregisterSettingsEvents();
        }

        private void ChangeQuality(int index)
        {
            if (_qualityIndex == index) return;

            _qualityIndex = index;
            _settingsData.QualityIndex = index;
            QualitySettings.SetQualityLevel(index);

            _saveSystem.SaveAsync();
        }

        private void ChangeVolume(float value)
        {
            if (Mathf.Approximately(_volume, value)) return;

            _volume = value;
            _settingsData.Volume = value;
            _audioSource.Volume = value;

            _saveSystem.SaveAsync();
        }

        private void ApplySettings()
        {
            QualitySettings.SetQualityLevel(_qualityIndex);
            _audioSource.Volume = _volume;
        }
    }
}
