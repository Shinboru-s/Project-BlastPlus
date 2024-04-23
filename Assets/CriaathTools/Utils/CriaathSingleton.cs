using UnityEngine;

namespace Criaath.MiniTools
{
    [DefaultExecutionOrder(-10)]
    public class CriaathSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            SetInstance();
        }
        protected virtual void SetInstance()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this as T;
        }
    }
}
