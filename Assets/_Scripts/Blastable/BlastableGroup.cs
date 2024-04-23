using System;
using System.Collections.Generic;
[Serializable]
public class BlastableGroup
{
    public int Id;
    public List<BlastableTile> Members = new();

    public BlastableGroup(int id)
    {
        Id = id;
    }
    public void Add(BlastableTile tile)
    {
        if (Members.Contains(tile)) return;
        tile.GroupId = Id;
        Members.Add(tile);
    }
}
