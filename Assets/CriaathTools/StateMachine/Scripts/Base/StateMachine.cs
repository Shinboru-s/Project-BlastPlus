using NaughtyAttributes;
using UnityEngine;

namespace Criaath.StateManagement
{
    public class StateMachine : MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector] public string[] StateNames => GetStateNames();
        private string[] GetStateNames()
        {
            string[] stateNames;
            if (_states == null)
            {
                _states = new State[0];
            }
            if (_states.Length == 0)
            {
                stateNames = new string[1];
                stateNames[0] = "Missing State";
                return stateNames;
            }

            stateNames = new string[_states.Length];
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] != null)
                    stateNames[i] = _states[i].StateName;
                else
                    stateNames[i] = "Missing State";
            }
            return stateNames;
        }
#endif

        [SerializeField][Dropdown("StateNames")] private string _startState;
        [SerializeField] private State[] _states;
        [SerializeField][ReadOnly] private State _currentState;
        private int _currentStateIndex;

        private void OnEnable()
        {
            if (_startState != null)
            {
                ChangeState(_startState);
            }
        }

        public void ChangeState(string stateName)
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (stateName == _states[i].StateName)
                {
                    ChangeState(_states[i]);
                    break;
                }
            }
        }

        public void ChangeState(State newState)
        {
            if (newState == _currentState)
                return;

            if (newState == null)
                return;

            if (_currentState != null)
                StartCoroutine(_currentState.Exit());

            _currentState = newState;
            SetStateIndex(_currentState);
            StartCoroutine(_currentState.Enter());
        }

        void SetStateIndex(State state)
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i].StateName == state.StateName)
                {
                    _currentStateIndex = i;
                    return;
                }
            }
            Debug.LogWarning(state.StateName + " state couldn't found in States array!");
            _currentStateIndex = -1;
        }

        public void NextState()
        {
            _currentStateIndex++;
            _currentStateIndex %= _states.Length;

            ChangeState(_states[_currentStateIndex]);
        }

        public void PreviousState()
        {
            _currentStateIndex--;
            _currentStateIndex += _states.Length;
            _currentStateIndex %= _states.Length;

            ChangeState(_states[_currentStateIndex]);
        }
    }

}

