using Criaath.MiniTools;
using Criaath.StateManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : CriaathSingleton<GroupManager>
{

    [SerializeField][ReadOnly] private List<BlastableGroup> _blastableGroups = new();
    [HideInInspector] public int NoneGroupId = 0;
    private int _nextGroupId = 1;
    private BlastableGroup _lastBlastedGroup;

    public BlastableGroup GetLastBlastedGroup() { return _lastBlastedGroup; }

    private void ClearGroup()
    {
        foreach (var group in _blastableGroups)
        {
            foreach (var blastableTile in group.Members)
            {
                blastableTile.SetAsGroupless();
            }
        }
        _blastableGroups.Clear();
    }

    public void CheckGroup()
    {
        ClearGroup();
        Vector2Int matrixSize = BlastableManager.Instance.GetMatrixSize();
        // Row check
        for (int y = 0; y < matrixSize.y; y++)
        {
            BlastableTile previousTile = null;
            for (int x = 0; x < matrixSize.x; x++)
            {
                BlastableTile currentTile = BlastableManager.Instance.GetBlastableTile(x, y);

                if (previousTile != null && currentTile != null && currentTile.IsSameColor(previousTile))
                    GroupThem(previousTile, currentTile);

                previousTile = currentTile;
            }
        }

        // Column check
        for (int x = 0; x < matrixSize.x; x++)
        {
            BlastableTile previousTile = null;
            for (int y = 0; y < matrixSize.y; y++)
            {
                BlastableTile currentTile = BlastableManager.Instance.GetBlastableTile(x, y);

                if (previousTile != null && currentTile != null && currentTile.IsSameColor(previousTile))
                    GroupThem(previousTile, currentTile);

                previousTile = currentTile;
            }
        }

    }

    private void GroupThem(BlastableTile tileOne, BlastableTile tileTwo)
    {
        if (IsInGroup(tileOne) && IsInGroup(tileTwo)) Merge(tileOne, tileTwo);
        else if (IsInGroup(tileOne)) AddToGroup(tileTwo, tileOne.GroupId);
        else if (IsInGroup(tileTwo)) AddToGroup(tileOne, tileTwo.GroupId);
        else CreateNewGroup(tileOne, tileTwo);
    }
    private void Merge(BlastableTile tileOne, BlastableTile tileTwo)
    {
        int groupId = tileOne.GroupId;
        int oldGroupId = tileTwo.GroupId;
        if (groupId == oldGroupId) return;
        BlastableGroup groupToMerge = GetBlastableGroup(oldGroupId);

        foreach (var tile in groupToMerge.Members)
        {
            AddToGroup(tile, groupId);
        }

        DeleteGroup(oldGroupId);
    }
    private void AddToGroup(BlastableTile tile, int groupId)
    {
        GetBlastableGroup(groupId).Add(tile);
    }
    private void CreateNewGroup(BlastableTile tileOne, BlastableTile tileTwo)
    {
        int newGroupId = GetNewGroupId();
        BlastableGroup newGroup = new BlastableGroup(newGroupId);

        newGroup.Add(tileOne);
        newGroup.Add(tileTwo);

        _blastableGroups.Add(newGroup);
    }
    private void DeleteGroup(int groupId)
    {
        BlastableGroup blastableGroup = GetBlastableGroup(groupId);
        _blastableGroups.Remove(blastableGroup);
    }


    public bool IsInGroup(BlastableTile blastTile)
    {
        return blastTile.GroupId != NoneGroupId;
    }

    public BlastableGroup GetBlastableGroup(int groupId)
    {
        foreach (var group in _blastableGroups)
        {
            if (group.Id == groupId) return group;
        }
        CriaathDebugger.LogError("Blastable Manager", Color.blue, $"Group id {groupId} was not found!", Color.white);
        return null;
    }
    private int GetNewGroupId()
    {
        _nextGroupId++;
        return _nextGroupId;
    }
    [SerializeField] private StateMachine _stateMachine;
    public void BlastGroup(int groupID)
    {
        BlastableGroup group = GetBlastableGroup(groupID);
        _lastBlastedGroup = group;
        foreach (var member in group.Members)
        {
            member.Blast();
        }
        _stateMachine.NextState();
    }


}
