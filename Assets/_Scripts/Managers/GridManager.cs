using Criaath.Extensions;
using Criaath.MiniTools;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : CriaathSingleton<GridManager>
{
    private List<BaseTile> _baseTiles = new List<BaseTile>();
    [SerializeField] private Grid _grid;
    [SerializeField][ReadOnly] private Vector2 _matrixOffset;
    [SerializeField][ReadOnly] private Vector2Int _matrixSize;

    private BaseTile[,] _baseGridMatrix;
    private BaseTile[] _topTiles;
    private int[] _tileCountColumn;
    private int[] _emptyTileCountsInColumn;
    private Vector2[,] _spawnPoints;

    public Action OnGridReady;

    //todo Delete this
    public List<BaseTile> GetBaseTiles() { return _baseTiles; }
    public Vector2Int GetMatrixSize() { return _matrixSize; }

    private void Start()
    {
        //?Criaath.Utils.ActionDelay(0.1f, PrepareGrid);
        //?BlastableManager.Instance.OnReadyToPlay += CheckAndFillTiles;
    }

    public void PrepareGrid()
    {
        CalculateMatrixOffsetAndSize();
        GenerateMatrix();
        CountColumns();
        FindTopTiles();
        GenerateSpawnPoints();
        //?OnGridReady?.Invoke();
    }
    public void CheckAndFillTiles()
    {
        CheckEmptyTiles();
        FillEmtyTiles();
    }

    public void AddBaseTile(BaseTile tile) => _baseTiles.Add(tile);
    public void RemoveBaseTile(BaseTile tile) => _baseTiles.Remove(tile);

    private void CalculateMatrixOffsetAndSize()
    {
        if (_baseTiles.IsNullOrEmpty())
        {
            CriaathDebugger.LogError("GridManager", Color.magenta, "_baseTiles list is null or empty", Color.white);
            return;
        }

        // for x calculation
        float smallest = _baseTiles[0].transform.position.x;
        float biggest = _baseTiles[0].transform.position.x;
        int xValue;
        int yValue;

        for (int i = 1; i < _baseTiles.Count; i++)
        {
            if (_baseTiles[i].transform.position.x < smallest)
            {
                smallest = _baseTiles[i].transform.position.x;
            }
            if (_baseTiles[i].transform.position.x > biggest)
            {
                biggest = _baseTiles[i].transform.position.x;
            }
        }

        _matrixOffset = new Vector2(smallest, 0);
        xValue = (int)(biggest - smallest);
        _matrixSize = new Vector2Int(xValue, 0);

        // for y calculation
        smallest = _baseTiles[0].transform.position.y;
        biggest = _baseTiles[0].transform.position.y;

        for (int i = 1; i < _baseTiles.Count; i++)
        {
            if (_baseTiles[i].transform.position.y < smallest)
            {
                smallest = _baseTiles[i].transform.position.y;
            }
            if (_baseTiles[i].transform.position.y > biggest)
            {
                biggest = _baseTiles[i].transform.position.y;
            }
        }

        _matrixOffset = new Vector2(_matrixOffset.x, smallest);
        yValue = (int)(biggest - smallest);
        _matrixSize = new Vector2Int(_matrixSize.x + 1, yValue + 1);

        CriaathDebugger.Log("Matrix offset", Color.magenta, _matrixOffset, Color.white);
        CriaathDebugger.Log("Matrix size", Color.magenta, _matrixSize, Color.white);
    }

    private void GenerateMatrix()
    {
        _baseGridMatrix = new BaseTile[_matrixSize.x, _matrixSize.y];
        foreach (BaseTile baseTile in _baseTiles)
        {
            int matrixPosX = (int)(baseTile.transform.position.x - _matrixOffset.x);
            int matrixPosY = _matrixSize.y - 1 - (int)(baseTile.transform.position.y - _matrixOffset.y);
            _baseGridMatrix[matrixPosX, matrixPosY] = baseTile;
            baseTile.MatrixPosition = new Vector2Int(matrixPosX, matrixPosY);
        }

        string matrixOutput = "";
        for (int y = 0; y < _matrixSize.y; y++)
        {
            for (int x = 0; x < _matrixSize.x; x++)
            {
                string matrixElement;
                if (_baseGridMatrix[x, y] == null) matrixElement = "o";
                else matrixElement = "x";
                matrixOutput += matrixElement;
            }
            matrixOutput += Environment.NewLine;
        }
        CriaathDebugger.Log("Matrix", Color.magenta, matrixOutput, Color.white);

    }

    private void FindTopTiles()
    {
        _topTiles = new BaseTile[_matrixSize.x];

        for (int x = 0; x < _matrixSize.x; x++)
        {
            for (int y = 0; y < _matrixSize.y; y++)
            {
                if (_baseGridMatrix[x, y] == null) continue;

                _topTiles[x] = _baseGridMatrix[x, y];
                break;
            }
        }

        string topTilePositions = "";
        for (int i = 0; i < _topTiles.Length; i++)
        {
            if (_topTiles[i] == null) topTilePositions += "none";
            else topTilePositions += _topTiles[i].transform.position;
            topTilePositions += " || ";
        }
        CriaathDebugger.Log("Top Tiles", Color.magenta, topTilePositions, Color.white);

    }

    private void CountColumns()
    {
        _tileCountColumn = new int[_matrixSize.x];
        Array.Clear(_tileCountColumn, 0, _tileCountColumn.Length);

        foreach (var tile in _baseTiles)
        {
            _tileCountColumn[tile.MatrixPosition.x]++;
        }

        string columnCounts = "";
        for (int i = 0; i < _tileCountColumn.Length; i++)
        {
            columnCounts += _tileCountColumn[i] + " || ";
        }
        CriaathDebugger.Log("Column Counts", Color.magenta, columnCounts, Color.white);
    }

    public void ResetEmptyTileCounts()
    {
        if (_emptyTileCountsInColumn == null) _emptyTileCountsInColumn = new int[_matrixSize.x];
        Array.Clear(_emptyTileCountsInColumn, 0, _emptyTileCountsInColumn.Length);

    }
    public void CheckEmptyTiles()
    {
        ResetEmptyTileCounts();

        foreach (var tile in _baseTiles)
        {
            if (tile.IsEmpty()) _emptyTileCountsInColumn[tile.MatrixPosition.x]++;
        }

        string emptyTilesCount = "";
        for (int i = 0; i < _emptyTileCountsInColumn.Length; i++)
        {
            emptyTilesCount += _emptyTileCountsInColumn[i] + " || ";
        }
        CriaathDebugger.Log("Empty Tile Counts In Columns", Color.magenta, emptyTilesCount, Color.white);
    }

    private void GenerateSpawnPoints()
    {
        float distanceBetweenTiles = _grid.cellGap.y + _grid.cellSize.y;
        CriaathDebugger.Log("Distance Between Tiles", Color.magenta, distanceBetweenTiles, Color.white);

        _spawnPoints = new Vector2[_matrixSize.x, _matrixSize.y];
        for (int x = 0; x < _matrixSize.x; x++)
        {
            if (_topTiles[x] == null) continue;

            Vector2 spawnPoint = _topTiles[x].transform.position;
            for (int y = 0; y < _matrixSize.y; y++)
            {
                spawnPoint += new Vector2(0, distanceBetweenTiles);
                _spawnPoints[x, y] = spawnPoint;
            }
        }

        string spawnPoints = "";
        for (int x = 0; x < _matrixSize.x; x++)
        {
            for (int y = 0; y < _matrixSize.y; y++)
            {
                spawnPoints += _spawnPoints[x, y] + " || ";
            }
            spawnPoints += Environment.NewLine;
        }
        CriaathDebugger.Log("Spawn Points", Color.magenta, spawnPoints, Color.white);
    }

    public void FillEmtyTiles()
    {
        for (int x = 0; x < _emptyTileCountsInColumn.Length; x++)
        {
            int numberOfTiles = _emptyTileCountsInColumn[x];
            for (int y = 0; y < _emptyTileCountsInColumn[x]; y++)
            {
                numberOfTiles--;
                BaseTile baseTile = GetValidTileInColumn(x, numberOfTiles);
                baseTile.SetEmpty(false);
                BlastableManager.Instance.Spawn(_spawnPoints[x, y], baseTile);
            }
        }
    }

    private BaseTile GetValidTileInColumn(int column, int orderNumber)
    {
        int tileCount = 0;
        for (int y = 0; y < _matrixSize.y; y++)
        {
            if (_baseGridMatrix[column, y] == null) continue;

            if (orderNumber == tileCount) return _baseGridMatrix[column, y];
            tileCount++;
        }

        CriaathDebugger.LogError("GridManager", Color.magenta, $"{orderNumber}. tile not found in column {column}", Color.white);
        return null;
    }

    public void SetTileEmpty(Vector2Int matrixPosition)
    {
        _baseGridMatrix[matrixPosition.x, matrixPosition.y].SetEmpty();
    }


    public void FindTilesToDrop()
    {

        for (int x = 0; x < _matrixSize.x; x++)
        {
            BaseTile emptyTile = null;
            for (int y = _matrixSize.y - 1; y >= 0; y--)
            {
                if (_baseGridMatrix[x, y] == null)
                    continue;

                if (emptyTile == null)
                {
                    if (_baseGridMatrix[x, y].IsEmpty())
                        emptyTile = _baseGridMatrix[x, y];
                }
                else
                {
                    if (_baseGridMatrix[x, y].IsEmpty())
                        continue;

                    BlastableManager.Instance.Move(new Vector2Int(x, y), emptyTile);
                    _baseGridMatrix[x, y].SetEmpty();
                    emptyTile.SetEmpty(false);

                    y = _matrixSize.y;
                    emptyTile = null;
                }

            }
        }


    }
}
