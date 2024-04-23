using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Criaath.StateManagement
{
    [ExecuteInEditMode]
    public class State : MonoBehaviour
    {
        [ValidateInput("CheckStateName")]
        public string StateName;
        private IEnumerator _updateStateCoroutine;


        [Header("Delay")]
        [SerializeField] private float _enterDelay = 0;
        [SerializeField] private float _exitDelay = 0;

        [Foldout("Events")] public UnityEvent OnEnter;
        [Foldout("Events")] public UnityEvent OnGoing;
        [Foldout("Events")] public UnityEvent OnExit;

        public IEnumerator Enter()
        {
            yield return new WaitForSeconds(_enterDelay);
            OnEnter?.Invoke();
            _updateStateCoroutine = UpdateState();
            StartCoroutine(_updateStateCoroutine);
        }

        public IEnumerator UpdateState()
        {
            while (true)
            {
                OnGoing?.Invoke();

                yield return new WaitForEndOfFrame();
            }
        }

        public virtual IEnumerator Exit()
        {
            yield return new WaitForSeconds(_exitDelay);
            StopCoroutine(_updateStateCoroutine);
            OnExit?.Invoke();

        }

        #region StateNameCheck
        void OnEnable()
        {
            if (string.IsNullOrEmpty(StateName))
            {
                StateName = gameObject.name;
            }
        }
        bool CheckStateName(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
                return false;

            return true;
        }
        #endregion
    }
}
