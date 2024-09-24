using Assets.EntryPoint;
using Assets.GameSettings;
using Assets.MVP;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour, IInitializer, IAudioSettings
    {
        [SerializeField] private AudioResource _mainMenuMusic;
        [SerializeField] private AudioResource _gameMusic;

        private AudioSource _audioSource;

        public float Volume 
        {
            get => _audioSource.volume; 
            set => _audioSource.volume = value;
        }

        public void Init(IResolver container)
        {
            var currentState = container.Resolve<StateView>();

            SetMusicForState(currentState);
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void SetMusicForState(StateView state)
        {
            switch (state)
            {
                case StateView.MainMenu:
                    _audioSource.resource = _mainMenuMusic;
                    break;
                case StateView.GameMenu:
                    _audioSource.resource = _gameMusic;
                    break;
                default:
                    Debug.LogWarning($"Неизвестное состояние: {state}");
                    return;
            }
        }
    }
}
