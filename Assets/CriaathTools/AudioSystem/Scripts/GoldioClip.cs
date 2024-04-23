using UnityEngine;

namespace Criaath.Goldio
{
    [CreateAssetMenu(fileName = "_GoldioClip", menuName = "Criaath/Goldio Clip")]
    public class GoldioClip : ScriptableObject
    {
        [SerializeField] private AudioClip m_Clip;
        public AudioClip Clip
        {
            private set => m_Clip = value;
            get => m_Clip;
        }

        public GoldioType Type;
        [Range(0, 1)] public float Volume = 0.5f;
        [Range(-3, 3)] public float Pitch = 1f;
        public bool Loop = false;

        public void Play()
        {
            GoldioManager.Instance.Play(this);
        }

    }
}
