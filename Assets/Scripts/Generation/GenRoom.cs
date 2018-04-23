using System;
using System.Collections.Generic;
using UnityEngine;

public class GenRoom {
	// ---Internal for generation only---
    // TODO make them package protected please
    
	public RectInt bounds = new RectInt();
	public HashSet<Vector2Int> doorsUp = new HashSet<Vector2Int>();
	public HashSet<Vector2Int> doorsDown = new HashSet<Vector2Int>();
	public HashSet<Vector2Int> doorsLeft = new HashSet<Vector2Int>();
	public HashSet<Vector2Int> doorsRight = new HashSet<Vector2Int>();

	// --- The final room genration result ---
    
	// The position of the anchor of the room in world space. This should be the top left corner of the room, but may be any point in the world.
	public Vector2Int roomPosition;
    // All positions are in room space relative to the room's anchor
	public Dictionary<Vector2Int, GenTile> tiles = new Dictionary<Vector2Int, GenTile>();
	public HashSet<Vector2Int> spawnpoints = new HashSet<Vector2Int>();
    public Objective objective = null;

    public float Distance(GenRoom r) {
        return Math.Abs(GetCenter().x - r.GetCenter().x) + Math.Abs(GetCenter().y - r.GetCenter().y);
		//float power = 2;
		//float dist = (float) Math.Pow(
		//	Math.Pow(GetCenter().x - r.GetCenter().x, power)
		//	+ Math.Pow(GetCenter().y - r.GetCenter().y, power),
		//	1 / power);
		//Debug.Log(bounds.center + " " + bounds + " " + dist);
        //return dist;
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