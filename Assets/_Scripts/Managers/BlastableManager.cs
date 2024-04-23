using Criaath;
using Criaath.MiniTools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlastableManager : CriaathSingleton<BlastableManager>
{
    [SerializeField] private TileColor[] _tileColors;
    [SerializeField] private RuneTile[] _tileSprites;
    [SerializeField] private GameObject _blasTilePrefab;

    private List<BaseTile> _baseTiles;
    private Vector2Int _matrixSize;
    private BlastableTile[,] _blastableMatrix;
    private ObjectPool<BlastableTile> _blastPool;

    public UnityEvent OnBlastSuccess;
    public Action OnReadyToPlay;
    public Action OnBlastFail;

    public Vector2Int GetMatrixSize() { return _matrixSize; }
    public BlastableTile GetBlastableTile(Vector2Int matrixPosition) { return _blastableMatrix[matrixPosition.x, matrixPosition.y]; }
    public BlastableTile GetBlastableTile(int x, int y) { return _blastableMatrix[x, y]; }

    private void Start()
    {
        //GridManager.Instance.OnGridReady += Initialize;
        // GroupManager.Instance.OnBlastCompleted += DropTopTiles;
    }
    public void Initialize()
    {
        _baseTiles = GridManager.Instance.GetBaseTiles();
        _matrixSize = GridManager.Instance.GetMatrixSize();
        GenerateMatrix();
        GenerateBlastablePool();
        //?OnReadyToPlay?.Invoke();
    }

    private void GenerateMatrix()
    {
        _blastableMatrix = new BlastableTile[_matrixSize.x, _matrixSize.y];
    }
    private void GenerateBlastablePool()
    {
        if (_blastPool != null) return;
        _blastPool = new ObjectPool<BlastableTile>(_blasTilePrefab, transform, _baseTiles.Count * 2);
    }
    public void CreateBlastables()
    {
        _blastPool.PushAllItems();
        foreach (var tile in _baseTiles)
        {
            Spawn(tile.transform.position, tile);
        }
    }
    public void Spawn(Vector2 position, BaseTile targetTile)
    {
        BlastableTile blastTile = _blastPool.Pull();

        blastTile.SetPosition(position);
        //blastTile.SetColor(GetRandomColor());
        blastTile.SetSprite(GetRandomSprite());
        blastTile.SetMatrixPosition(targetTile.MatrixPosition);
        AssignMatrixPosition(blastTile);
        blastTile.SetAsGroupless();
        blastTile.gameObject.SetActive(true);
        blastTile.Move(targetTile.transform.position);
    }
    public void Move(Vector2Int matrixPosition, BaseTile target)
    {
        BlastableTile blastTile = _blastableMatrix[matrixPosition.x, matrixPosition.y];

        blastTile.SetMatrixPosition(target.MatrixPosition);
        AssignMatrixPosition(blastTile);
        blastTile.Move(target.transform.position);
    }
    private TileColor GetRandomColor()
    {
        int randomInt = UnityEngine.Random.Range(0, _tileColors.Length);
        return _tileColors[randomInt];
    }
    private RuneTile GetRandomSprite()
    {
        int randomInt = UnityEngine.Random.Range(0, _tileSprites.Length);
        return _tileSprites[randomInt];
    }

    public void AssignMatrixPosition(BlastableTile tile)
    {
        _blastableMatrix[tile.MatrixPosition.x, tile.MatrixPosition.y] = tile;
    }


    public void TryToBlast(Vector2Int matrixPosition)
    {
        BlastableTile tile = _blastableMatrix[matrixPosition.x, matrixPosition.y];

        if (CanBlast(tile))
        {
            GroupManager.Instance.BlastGroup(tile.GroupId);
            OnBlastSuccess?.Invoke();
        }
        else
            OnBlastFail?.Invoke();
    }

    private bool CanBlast(BlastableTile tile)
    {
        return tile != null && GroupManager.Instance.IsInGroup(tile);
    }

    public void ResetTile(BlastableTile tile)
    {
        tile.gameObject.SetActive(false);
        tile.transform.localScale = Vector3.one;
        tile.SetAsGroupless();
        _blastPool.PushItem(tile);
    }

    private void DropTopTiles()
    {
        // hangi grubun patladigini elde et
        BlastableGroup group = GroupManager.Instance.GetLastBlastedGroup();
        Vector2Int[] matrixPositions = new Vector2Int[group.Members.Count];

        for (int i = 0; i < matrixPositions.Length; i++)
        {
            matrixPositions[i] = group.Members[i].MatrixPosition;
        }
        //her bir eleman icin

        //  y 0a ulasana kadar yukari cik
        //  eger herhangi bir tile elde edersen bosluga ilerlet
        //  ayni zamanda target olarak hareket edecek tile olarak sec
        //  yukaridaki islemi bu tile icin uygula
    }
}
