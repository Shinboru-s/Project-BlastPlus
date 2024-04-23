using Criaath.MiniTools;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criaath.Goldio
{
    public class GoldioManager : CriaathSingleton<GoldioManager>
    {
        [Foldout("Pool Settings")][SerializeField] private GameObject _goldioSourcePrefab;
        [Foldout("Pool Settings")][SerializeField] private int _defaultPoolSize;

        [OnValueChanged("SetVolume")]
        [Foldout("Volume Settings")]
        [SerializeField][Range(0, 1)] private float _sfxVolume = 0.5f;

        [OnValueChanged("SetVolume")]
        [Foldout("Volume Settings")]
        [SerializeField][Range(0, 1)] private float _musicVolume = 0.5f;

        private ObjectPool<GoldioSource> _goldioSourcePool;
        private List<GoldioSource> _goldioSourcesInUse;

        new void Awake()
        {
            SetInstance();
            _goldioSourcePool = new ObjectPool<GoldioSource>(_goldioSourcePrefab, transform, _defaultPoolSize);
            _goldioSourcesInUse = new List<GoldioSource>();
        }

        public void Play(GoldioPlayer goldioPlayer)
        {
            GoldioSource source = PrepareAudioSource(goldioPlayer.GoldioClip);
            goldioPlayer.SetAudioSource(source);
            source.Play();
            _goldioSourcesInUse.Add(source);
        }

        public void Play(GoldioClip goldioClip)
        {
            GoldioSource source = PrepareAudioSource(goldioClip);
            source.Play();
            _goldioSourcesInUse.Add(source);
        }

        public void Stop(GoldioPlayer goldioPlayer)
        {
            GoldioSource source = goldioPlayer.GetAudioSource();
            source.Stop();
            goldioPlayer.SetAudioSource(null);

        }

        private GoldioSource PrepareAudioSource(GoldioClip goldioClip)
        {
            if (goldioClip == null) return null;
            if (goldioClip.Clip == null) return null;

            GoldioSource source = _goldioSourcePool.Pull();
            source.gameObject.SetActive(true);

            source.SetClipSettings(goldioClip);
            UpdateSourceVolume(source);

            return source;
        }

        //todo audioClip icin de play metodu

        //todo sfx ve music volume degerleri degistiginde sesler de etkilenmeli

        public void StopAudioSource(GoldioSource audioSource)
        {
            _goldioSourcesInUse.Remove(audioSource);
            _goldioSourcePool.PushItem(audioSource);
        }

        private IEnumerator ActionDelay(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);

            onEnd();
        }

        //? Volume Settings
        private void SetVolume()
        {
            SetVolume(GoldioType.SFX, _sfxVolume);
            SetVolume(GoldioType.Music, _musicVolume);
        }

        public void SetVolume(GoldioType type, float volume)
        {
            volume = Math.Clamp(volume, 0f, 1f);

            if (type == GoldioType.Music)
                _musicVolume = volume;
            else if (type == GoldioType.SFX)
                _sfxVolume = volume;

            for (int i = 0; i < _goldioSourcesInUse.Count; i++)
            {
                if (_goldioSourcesInUse[i].CheckType(type))
                    _goldioSourcesInUse[i].SetVolume(volume);
            }
        }

        public void SetVolume(GoldioType type, float volume, float validityDuration)
        {
            float oldVolume = 1;
            if (type == GoldioType.Music)
                oldVolume = _musicVolume;
            else if (type == GoldioType.SFX)
                oldVolume = _sfxVolume;

            SetVolume(type, volume);
            StartCoroutine(ActionDelay(validityDuration, () =>
            {
                SetVolume(type, oldVolume);
            }));
        }

        public void UpdateSourceVolume(GoldioSource source)
        {
            if (source.CheckType(GoldioType.Music))
                source.SetVolume(_musicVolume);
            else
                source.SetVolume(_sfxVolume);
        }
    }
}
