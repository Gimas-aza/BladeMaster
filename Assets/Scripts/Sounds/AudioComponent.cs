using Assets.EntryPoint;
using Assets.MVP;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour, IInitializer
    {
        [SerializeField] private AudioResource _mainMenuMusic;
        [SerializeField] private AudioResource _gameMusic;

        private AudioSource _audioSource;

        public float Volume { get => _audioSource.volume; set => _audioSource.volume = value; }

        public void Init(StateView currentState)
        {
            if (currentState == StateView.MainMenu) _audioSource.resource = _mainMenuMusic;
            if (currentState == StateView.GameMenu) _audioSource.resource = _gameMusic;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}
