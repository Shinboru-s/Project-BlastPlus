using Criaath.MiniTools;
using Criaath.StateManagement;
using NaughtyAttributes;
using UnityEngine;

public class AnimationManager : CriaathSingleton<AnimationManager>
{
    [SerializeField] private StateMachine _stateManager;
    [ReadOnly][SerializeField] private int _tileCountInAnimation;
    private bool _checkAnimations;

    public void ToggleCheckAnimations(bool state) => _checkAnimations = state;
    private bool CanCheckAnimation() { return _checkAnimations; }

    public void AnimationStarted()
    {
        _tileCountInAnimation++;
    }
    public void AnimationEnded()
    {
        _tileCountInAnimation--;
        CheckTileCountInAnimation();
    }
    private void CheckTileCountInAnimation()
    {
        if (_tileCountInAnimation > 0) return;
        if (CanCheckAnimation() is not true) return;

        //todo on animation end
        _stateManager.NextState();
    }


}
