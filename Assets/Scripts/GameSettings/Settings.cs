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
        private IResolver _resolver;
        private ISaveSystem _saveSystem;
        private ISettingsData _settingsData;

        public void Init(IResolver container)
        {
            _resolver = container;
            _saveSystem = container.Resolve<ISaveSystem>();
            _settingsData = container.Resolve<ISettingsData>();

            _qualityIndex = _settingsData.QualityIndex;
            _volume = _settingsData.Volume;

            ChangeQuality(_qualityIndex);
            ChangeVolume(_volume);
        }

        public void SubscribeToEvents(IResolver container)
        {
            var uiEvents = container.Resolve<IUIEvents>();
            uiEvents.ChangeQuality += ChangeQuality;
            uiEvents.ChangeVolume += ChangeVolume;

            uiEvents.CurrentQuality?.Invoke(_qualityIndex);
            uiEvents.CurrentVolume?.Invoke(_volume);
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
            if (_audioSource == null)
                _audioSource = _resolver.Resolve<AudioComponent>();
            _audioSource.Volume = value;

            _saveSystem.SaveAsync();
        }
    }
}
