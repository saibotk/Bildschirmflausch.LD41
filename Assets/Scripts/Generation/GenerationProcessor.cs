using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationProcessor {
    public enum ExtendedTileType {
        BorderOuter, BorderInner, BorderSingle, Ground, Door, Rock, RockL, RockU, RockR, RockD, RockLU, RockLR, RockLD, RockUR, RockUD, RockRD, RockLURD, RockLUD, RockLUR, RockURD, RockLRD
    }
    Dictionary<ExtendedTileType, GameObject> prefabs;
    public GenerationProcessor(Dictionary<ExtendedTileType, GameObject> prefabs) {
        this.prefabs = prefabs;
    }

    public GameObject ProcessRoom(Dictionary<Vector2Int, GenTile> tiles) {
        GameObject root = new GameObject {
            name = "Room"
        };
		foreach ( Vector2Int v in tiles.Keys ) {
			ExtendedTileType type = ExtendedTileType.Ground;
			int rotation = 0;
            switch ( tiles[v].type ) {
                case Room.TileType.WALL:
					type = getCorrectWallType(tiles, v);
					switch (type)
					{
						case ExtendedTileType.BorderSingle:
							switch (tiles[v].position)
							{
								case GenTile.Position.BOTTOM:
									rotation = 180;
									break;
								case GenTile.Position.LEFT:
									rotation = 90;
									break;
								case GenTile.Position.TOP:
									rotation = 0;
									break;
								case GenTile.Position.RIGHT:
									rotation = 270;
									break;
							}
							break;
						case ExtendedTileType.BorderInner:
							switch (tiles[v].position)
							{
                                case GenTile.Position.BOTTOM_LEFT:
                                    rotation = 90;
                                    break;
                                case GenTile.Position.TOP_LEFT:
                                    rotation = 0;
                                    break;
								case GenTile.Position.TOP_RIGHT:
									rotation = 270;
									break;
								case GenTile.Position.BOTTOM_RIGHT:
									rotation = 180;
									break;
							}
							break;
					}
                    break;
                case Room.TileType.GROUND:
                    type = ExtendedTileType.Ground;
                    break;
                case Room.TileType.DOOR:
                    type = ExtendedTileType.Door;
                    break;
                case Room.TileType.ROCK:
					type = getCorrectRockType(tiles, v);
                    break;
            }

			CreateGOFromType(v, rotation, tiles[v].type, type, root);
        }

        return root;
    }

	private GameObject CreateGOFromType(Vector2 v, int rotation, Room.TileType type, ExtendedTileType t, GameObject root) {
        GameObject tmp = null;
		if (type == Room.TileType.ROCK || type == Room.TileType.WALL)
			CreateGOFromType(v, 0, Room.TileType.GROUND, ExtendedTileType.Ground, root);
        if ( prefabs.ContainsKey(t) && root != null ) {
            tmp = Object.Instantiate(prefabs[t], root.transform);
            tmp.transform.position = v;
			tmp.transform.Rotate(new Vector3(0, 0, rotation));
        }
        return tmp;
    }

	private int CountSpecificNeighbours(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position, Room.TileType type) {
		int counter = 0;
		Vector2Int toCheck = position + new Vector2Int(0, -1);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == type)
			counter++;
		toCheck = position + new Vector2Int(-1, 0);
        if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == type)
            counter++;
		toCheck = position + new Vector2Int(0, 1);
        if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == type)
            counter++;
		toCheck = position + new Vector2Int(1, 0);
        if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == type)
            counter++;
		return counter;
	}

	private ExtendedTileType getCorrectWallType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position){
		int groundNumber = CountSpecificNeighbours(tiles, position, Room.TileType.GROUND) + CountSpecificNeighbours(tiles, position, Room.TileType.ROCK);
		switch(groundNumber){
			case 0:
				return ExtendedTileType.BorderInner;
			case 2:
                return ExtendedTileType.BorderOuter;
			default:
				return ExtendedTileType.BorderSingle;
		}
	}

	private ExtendedTileType getCorrectRockType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position){

		ExtendedTileType type = ExtendedTileType.Rock;

		bool left = false;
        bool top = false;
        bool right = false;
        bool bottom = false;

		Vector2Int toCheck = position + new Vector2Int(0, -1);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			bottom = true;
        toCheck = position + new Vector2Int(-1, 0);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			left = true;
        toCheck = position + new Vector2Int(0, 1);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			top = true;
        toCheck = position + new Vector2Int(1, 0);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
            right = true;

		if (top && !right && !left && !bottom)
        {
            return ExtendedTileType.RockU;
        }
        if (left && !right && !bottom && !top)
        {
			return ExtendedTileType.RockL;
        }
        if (right && !bottom && !left && !top)
        {
			return ExtendedTileType.RockR;
        }
        if (bottom && !right && !left && !top)
        {
			return ExtendedTileType.RockD;
        }
        if (left && top && !bottom && !right)
        {
			return ExtendedTileType.RockLU;
        }
        if (left && right && !top && !bottom)
        {
			return ExtendedTileType.RockLR;
        }
        if (left && bottom && !right && !top)
        {
			return ExtendedTileType.RockLD;
        }
        if (top && right && !left && !bottom)
        {
			return ExtendedTileType.RockUR;
        }
        if (top && bottom && !left && !right)
        {
			return ExtendedTileType.RockUD;
        }
        if (right && bottom && !top && !left)
        {
			return ExtendedTileType.RockRD;
        }
        if (left && top && bottom && !right)
        {
			return ExtendedTileType.RockLUD;
        }
        if (left && top && right && !bottom)
        {
			return ExtendedTileType.RockLUR;
        }
        if (top && right && bottom && !left)
        {
			return ExtendedTileType.RockURD;
        }
        if (left && right && bottom && !top)
        {
			return ExtendedTileType.RockLRD;
        }
        if (left && top && right && bottom)
        {
			return ExtendedTileType.RockLURD;
        }
		return type;
	}
}
