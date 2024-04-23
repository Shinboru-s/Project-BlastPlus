using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class BlastableTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [ReadOnly] public Vector2Int MatrixPosition;
    [ReadOnly] public int GroupId;

    private TileColor _tileColor;
    private RuneTile _runeTile;
    private Sequence _animationSequence;

    public void SetColor(TileColor color)
    {
        _tileColor = color;
        _spriteRenderer.color = color.Color;
    }
    public void SetSprite(RuneTile runeTile)
    {
        _runeTile = runeTile;
        _spriteRenderer.sprite = _runeTile.Sprite;

    }
    public bool IsSameColor(BlastableTile blastTile)
    {
        return blastTile.GetTileName() == GetTileName();
    }

    public string GetTileName()
    {
        return _runeTile.name;
    }
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void Move(Vector2 position)
    {
        AnimationManager.Instance.AnimationStarted();
        float distance = Vector2.Distance(transform.position, position);
        transform.DOMove(position, distance / BlastableAnimationInfo.DropSpeed).SetEase(BlastableAnimationInfo.DropEase).OnComplete(() =>
        {
            AnimationManager.Instance.AnimationEnded();
        });
    }
    public void SetMatrixPosition(Vector2Int matrixPosition)
    {
        MatrixPosition = matrixPosition;
        _spriteRenderer.sortingOrder = 100 - matrixPosition.y;
    }
    public void Blast()
    {
        AnimationManager.Instance.AnimationStarted();
        _animationSequence = DOTween.Sequence();

        _animationSequence.Append(transform.DOScale(Vector2.zero, BlastableAnimationInfo.BlastTime).SetEase(BlastableAnimationInfo.BlastEase).OnComplete(() =>
        {
            BlastableManager.Instance.ResetTile(this);
            GridManager.Instance.SetTileEmpty(MatrixPosition);
            AnimationManager.Instance.AnimationEnded();
        }));
    }
    public void SetAsGroupless()
    {
        GroupId = GroupManager.Instance.NoneGroupId;
    }

    public Sequence GetAnimationSequence()
    {
        return _animationSequence;
    }
}
