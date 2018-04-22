using System;
using System.Collections.Generic;
using UnityEngine;

public class GenRoom {
    public RectInt bounds = new RectInt();
    public Dictionary<Vector2Int, Room.TileType> tiles = new Dictionary<Vector2Int, Room.TileType>();
    public HashSet<Vector2Int> doorsUp = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> doorsDown = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> doorsLeft = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> doorsRight = new HashSet<Vector2Int>();

    public float Distance(GenRoom r) {
        return Math.Abs(GetCenter().x - r.GetCenter().x) + Math.Abs(GetCenter().y - r.GetCenter().y);
    }

    public Vector2Int GetCenter() {
        return new Vector2Int(( int ) bounds.center.x, ( int ) bounds.center.y);
    }

    public HashSet<Vector2Int> AllDoors() {
        HashSet<Vector2Int> ret = new HashSet<Vector2Int>();
        ret.UnionWith(doorsUp);
        ret.UnionWith(doorsDown);
        ret.UnionWith(doorsLeft);
        ret.UnionWith(doorsRight);
        return ret;
    }
}