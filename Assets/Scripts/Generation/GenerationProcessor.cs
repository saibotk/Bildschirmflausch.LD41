﻿using System.Collections;
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
            rotation = getCorrectWallRotation(type, tiles[v].position);
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
        CreateGOFromType(v, rotation, type, root);
      }
      return root;
    }

    private GameObject CreateGOFromType(Vector2 v, int rotation, ExtendedTileType t, GameObject root) {
        GameObject tmp = null;
		//if (t == ExtendedTileType.BorderInner || t == ExtendedTileType.BorderOuter || t == ExtendedTileType.BorderInner)
		//	CreateGOFromType(v, rotation, ExtendedTileType.Ground, root);
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

  private int getCorrectWallRotation(ExtendedTileType type, GenTile.Position position) {
    int rotation = 0;
    switch (type) {
      case ExtendedTileType.BorderSingle:
        switch (position) {
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
        switch (position) {
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
      return rotation;
  }

	private ExtendedTileType getCorrectRockType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position) {
    int meta = 0;
		Vector2Int toCheck = position + new Vector2Int(0, -1);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			meta += 1;
        toCheck = position + new Vector2Int(-1, 0);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			meta += 2;
        toCheck = position + new Vector2Int(0, 1);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
			meta += 4;
        toCheck = position + new Vector2Int(1, 0);
		if (tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK)
      meta += 8;

    switch(meta) {
      case 1:
        return ExtendedTileType.RockD;
      case 2:
        return ExtendedTileType.RockL;
      case 3:
        return ExtendedTileType.RockLD;
      case 4:
        return ExtendedTileType.RockU;
      case 5:
        return ExtendedTileType.RockUD;
      case 6:
        return ExtendedTileType.RockLU;
      case 7:
        return ExtendedTileType.RockLUD;
      case 8:
        return ExtendedTileType.RockR;
      case 9:
        return ExtendedTileType.RockRD;
      case 10:
        return ExtendedTileType.RockLR;
      case 11:
        return ExtendedTileType.RockLRD;
      case 12:
        return ExtendedTileType.RockUR;
      case 13:
        return ExtendedTileType.RockURD;
      case 14:
        return ExtendedTileType.RockLUR;
      case 15:
        return ExtendedTileType.RockLURD;
      default:
        return ExtendedTileType.Rock;
    }
	}
}
