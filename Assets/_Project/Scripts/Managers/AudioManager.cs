using System.Collections.Generic;
using Studio.Core.Services;
using Studio.Data;
using Studio.Systems.Settings;
using UnityEngine;

namespace Studio.Managers
{
    public sealed class AudioManager : IManager
    {
        private readonly AudioConfig _config;
        private readonly SettingsSystem _settings;
        private AudioSource _musicSource;
        private readonly Queue<AudioSource> _sfxPool = new();
        private GameObject _audioRoot;

        public AudioManager(AudioConfig config, SettingsSystem settings)
        {
            _config = config;
            _settings = settings;
        }

        public void Initialize()
        {
            _audioRoot = new GameObject("AudioManager");
            Object.DontDestroyOnLoad(_audioRoot);

            _musicSource = _audioRoot.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            for (var i = 0; i < _config.SfxPoolSize; i++)
            {
                var source = _audioRoot.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _sfxPool.Enqueue(source);
            }

            ApplyVolumes();
            if (_config.DefaultMusic != null)
            {
                PlayMusic(_config.DefaultMusic);
            }
        }

        public void Shutdown()
        {
            if (_audioRoot != null)
            {
                Object.Destroy(_audioRoot);
            }

            _sfxPool.Clear();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || _musicSource == null)
            {
                return;
            }

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null || _sfxPool.Count == 0)
            {
                return;
            }

            var source = _sfxPool.Dequeue();
            source.PlayOneShot(clip);
            _sfxPool.Enqueue(source);
        }

        public void ApplyVolumes()
        {
            if (_musicSource == null || _settings == null)
            {
                return;
            }

            var master = _settings.MasterVolume.Value;
            _musicSource.volume = master * _settings.MusicVolume.Value;
        }
    }
}
