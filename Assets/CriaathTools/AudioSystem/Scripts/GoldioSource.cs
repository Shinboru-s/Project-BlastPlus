using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Criaath.Goldio
{
    public class GoldioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private GoldioType _type;
        private bool _isPlaying;
        private float _defaultVolume;
        private float _volumeMultiplier = 1;
        private IEnumerator _stopCoroutine;

        public void SetClipSettings(GoldioClip goldioClip)
        {
            _audioSource.clip = goldioClip.Clip;
            _audioSource.pitch = goldioClip.Pitch;
            _audioSource.loop = goldioClip.Loop;
            _audioSource.volume = goldioClip.Volume;
            _defaultVolume = goldioClip.Volume;
            _type = goldioClip.Type;
        }

        public void SetVolume(float volume)
        {
            _volumeMultiplier = volume;
            _audioSource.volume = _defaultVolume * volume;
        }

        public bool CheckType(GoldioType type) { return _type == type; }
        public bool IsPlaying() { return _isPlaying; }

        public void Play()
        {
            if (IsPlaying())
                _audioSource.Stop();

            _audioSource.Play();
            _isPlaying = true;

            if (_audioSource.loop == false)
            {
                if (_stopCoroutine != null) StopCoroutine(_stopCoroutine);

                _stopCoroutine = ActionDelay(_audioSource.clip.length, () =>
                {
                    _isPlaying = false;
                    _audioSource.clip = null;
                    GoldioManager.Instance.StopAudioSource(this);
                });

                StartCoroutine(_stopCoroutine);
            }
        }

        public void Stop()
        {
            if (IsPlaying() is not true) return;

            StopCoroutine(_stopCoroutine);
            _audioSource.Stop();
            _isPlaying = false;
            _audioSource.clip = null;
            GoldioManager.Instance.StopAudioSource(this);
        }

        public void FadeOut(float seconds)
        {
            _audioSource.DOFade(0, seconds);
        }

        public void FadeIn(float seconds)
        {
            _audioSource.volume = 0;
            _audioSource.DOFade(_defaultVolume * _volumeMultiplier, seconds);
        }

        public void SetRandomPitch(float range)
        {
            float pitch = _audioSource.pitch;
            _audioSource.pitch = UnityEngine.Random.Range(pitch - range, pitch + range);
        }
        public void SetTempRandomPitch(float pitcRange)
        {
            SetRandomPitch(pitcRange);

            float ogPitch = _audioSource.pitch;
            ActionDelay(_audioSource.clip.length, () =>
            {
                _audioSource.pitch = ogPitch;
            });
        }
        private IEnumerator ActionDelay(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);

            onEnd();
        }
    }
}
