using NaughtyAttributes;
using System;
using UnityEngine;

namespace Criaath.Goldio
{
    public class GoldioPlayer : MonoBehaviour
    {
        [SerializeField] private GoldioClip m_GoldioClip;
        private GoldioSource _audioSource;

        public GoldioClip GoldioClip
        {
            private set => m_GoldioClip = value;
            get => m_GoldioClip;
        }

        [Tooltip("Play audio in the selected event")]
        [Foldout("Settings")][SerializeField, Space(5)] private bool _autoPlay;
        [Tooltip("Select which event to play at")]
        [ShowIf("_autoPlay")][Foldout("Settings")][SerializeField] private PlayOnThis _playOn;
        private Action PlayAnimation;

        #region Built-in
        private void Awake()
        {
            if (!_autoPlay) return;
            PlayAnimation += Play;

            if (_playOn == PlayOnThis.Awake)
                PlayAnimation?.Invoke();
        }

        private void OnEnable()
        {
            if (_playOn == PlayOnThis.OnEnable)
                PlayAnimation?.Invoke();
        }
        private void Start()
        {
            if (_playOn == PlayOnThis.Start)
                PlayAnimation?.Invoke();
        }
        private void OnDisable()
        {
            if (_playOn == PlayOnThis.OnDisable)
                PlayAnimation?.Invoke();
        }
        private void OnDestroy()
        {
            if (_playOn == PlayOnThis.OnDestroy)
                PlayAnimation?.Invoke();
        }
        #endregion


        public void Play()
        {
            GoldioManager.Instance.Play(this);
        }
        public void Stop()
        {
            GoldioManager.Instance.Stop(this);
        }

        public void SetAudioSource(GoldioSource audioSource)
        {
            _audioSource = audioSource;
        }
        public GoldioSource GetAudioSource()
        {
            return _audioSource;
        }
        public void SetClip(GoldioClip clip)
        {
            GoldioClip = clip;
        }

        public void FadeOut(float seconds)
        {
            _audioSource.FadeOut(seconds);
        }
        public void FadeOut() => FadeOut(1f);

        public void FadeIn(float seconds)
        {
            _audioSource.FadeIn(seconds);
        }
        public void FadeIn() => FadeIn(1f);

        public void SetRandomPitch(float range)
        {
            _audioSource.SetRandomPitch(range);
        }
        public void PlayWithRandomPitch(float range)
        {
            Play();
            _audioSource.SetTempRandomPitch(range);
        }
    }
}
