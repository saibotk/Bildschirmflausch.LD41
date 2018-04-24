using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationProcessor {
    public enum ExtendedTileType {
        BorderOuter, BorderInner, BorderSingle, Ground0, Ground1, Ground2, Ground3, DoorInner, DoorOuter, Rock, RockL, RockU, RockR, RockD, RockLU, RockLR, RockLD, RockUR, RockUD, RockRD, RockLURD, RockLUD, RockLUR, RockURD, RockLRD, Flag
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
            ExtendedTileType type = GetRandomGroundType();
            int rotation = 0;
            switch ( tiles[v].type ) {
                case Room.TileType.WALL:
                    type = GetCorrectWallType(tiles, v);
                    rotation = GetCorrectWallRotation(tiles, v, type);
                    break;
                case Room.TileType.GROUND:
                    type = GetRandomGroundType();
                    break;
                case Room.TileType.DOOR:
                    type = GetCorrectDoorType(tiles, v);
                    rotation = GetCorrectDoorRotation(type, tiles, v);
                    break;
                case Room.TileType.ROCK:
                    type = GetCorrectRockType(tiles, v);
                    break;
            }
            GameObject go = CreateGOFromType(v, rotation, tiles[v].type, type, root);
            // Todo dirty hack
            if ( go.tag == "door" ) {
                go.GetComponent<Door>().SetToOuter(GenerationProcessor.GetDirectionVector(tiles[v].position));
            }
        }
        return root;
    }

    public GameObject CreateGOFromType(Vector2 v, int rotation, Room.TileType type, ExtendedTileType t, GameObject root) {
        GameObject tmp = null;
        if ( type != Room.TileType.GROUND )
            CreateGOFromType(v, 0, Room.TileType.GROUND, GetRandomGroundType(), root);
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
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == type )
            counter++;
        toCheck = position + new Vector2Int(-1, 0);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == type )
            counter++;
        toCheck = position + new Vector2Int(0, 1);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == type )
            counter++;
        toCheck = position + new Vector2Int(1, 0);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == type )
            counter++;
        return counter;
    }

    private ExtendedTileType GetCorrectWallType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position) {
        int groundNumber = CountSpecificNeighbours(tiles, position, Room.TileType.GROUND) + CountSpecificNeighbours(tiles, position, Room.TileType.ROCK);
        switch ( groundNumber ) {
            case 0:
                return ExtendedTileType.BorderInner;
            case 2:
                return ExtendedTileType.BorderOuter;
            default:
                return ExtendedTileType.BorderSingle;
        }
    }

    private int GetCorrectWallRotation(Dictionary<Vector2Int, GenTile> tiles, Vector2Int v, ExtendedTileType type) {
        switch ( type ) {
            case ExtendedTileType.BorderSingle:
                Vector2Int toCheck = v + new Vector2Int(0, -1);
                if(tiles.ContainsKey(toCheck) && (tiles[toCheck].type == Room.TileType.GROUND || tiles[toCheck].type == Room.TileType.GROUND))
                  return 0;
                toCheck = v + new Vector2Int(-1, 0);
                if(tiles.ContainsKey(toCheck) && (tiles[toCheck].type == Room.TileType.GROUND || tiles[toCheck].type == Room.TileType.GROUND))
                  return 270;
                toCheck = v + new Vector2Int(0, 1);
                if(tiles.ContainsKey(toCheck) && (tiles[toCheck].type == Room.TileType.GROUND || tiles[toCheck].type == Room.TileType.GROUND))
                  return 180;
                toCheck = v + new Vector2Int(1, 0);
                if(tiles.ContainsKey(toCheck) && (tiles[toCheck].type == Room.TileType.GROUND || tiles[toCheck].type == Room.TileType.GROUND))
                  return 90;
                break;
            case ExtendedTileType.BorderInner:
                toCheck = v + new Vector2Int(1, -1);
                if(tiles.ContainsKey(toCheck) && tiles[toCheck].type != Room.TileType.WALL)
                  return 0;
                toCheck = v + new Vector2Int(1, 1);
                if(tiles.ContainsKey(toCheck) && tiles[toCheck].type != Room.TileType.WALL)
                  return 90;
                toCheck = v + new Vector2Int(-1, 1);
                if(tiles.ContainsKey(toCheck) && tiles[toCheck].type != Room.TileType.WALL)
                  return 180;
                toCheck = v + new Vector2Int(-1, -1);
                if(tiles.ContainsKey(toCheck) && tiles[toCheck].type != Room.TileType.WALL)
                  return 270;
                break;
            case ExtendedTileType.BorderOuter:
                Vector2Int toCheck1 = v + new Vector2Int(0, -1);
                Vector2Int toCheck2 = v + new Vector2Int(-1, 0);
                if(tiles.ContainsKey(toCheck1) && tiles.ContainsKey(toCheck2) && tiles[toCheck1].type == Room.TileType.WALL && tiles[toCheck2].type == Room.TileType.WALL)
                  return 90;
                toCheck1 = v + new Vector2Int(0, 1);
                if(tiles.ContainsKey(toCheck1) && tiles.ContainsKey(toCheck2) && tiles[toCheck1].type == Room.TileType.WALL && tiles[toCheck2].type == Room.TileType.WALL)
                  return 0;
                toCheck2 = v + new Vector2Int(1, 0);
                if(tiles.ContainsKey(toCheck1) && tiles.ContainsKey(toCheck2) && tiles[toCheck1].type == Room.TileType.WALL && tiles[toCheck2].type == Room.TileType.WALL)
                  return 270;
                toCheck1 = v + new Vector2Int(0, -1);
                if(tiles.ContainsKey(toCheck1) && tiles.ContainsKey(toCheck2) && tiles[toCheck1].type == Room.TileType.WALL && tiles[toCheck2].type == Room.TileType.WALL)
                  return 180;
                break;
        }
        return 0;
    }

    private ExtendedTileType GetCorrectRockType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position) {
        int meta = 0;
        Vector2Int toCheck = position + new Vector2Int(0, -1);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK )
            meta += 1;
        toCheck = position + new Vector2Int(-1, 0);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK )
            meta += 2;
        toCheck = position + new Vector2Int(0, 1);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK )
            meta += 4;
        toCheck = position + new Vector2Int(1, 0);
        if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.ROCK )
            meta += 8;

        switch ( meta ) {
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

    private ExtendedTileType GetCorrectDoorType(Dictionary<Vector2Int, GenTile> tiles, Vector2Int position) {
        int neighbourDoors = CountSpecificNeighbours(tiles, position, Room.TileType.DOOR);
        switch ( neighbourDoors ) {
            case 1:
                return ExtendedTileType.DoorOuter;
            default:
                return ExtendedTileType.DoorInner;
        }
    }

    private int GetCorrectDoorRotation(ExtendedTileType type, Dictionary<Vector2Int, GenTile> tiles, Vector2Int position) {
        switch ( type ) {
            case ExtendedTileType.DoorOuter:
                Vector2Int toCheck = position + new Vector2Int(0, -1);
                if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.DOOR )
                    return 270;
                toCheck = position + new Vector2Int(-1, 0);
                if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.DOOR )
                    return 180;
                toCheck = position + new Vector2Int(0, 1);
                if ( tiles.ContainsKey(toCheck) && tiles[toCheck].type == Room.TileType.DOOR )
                    return 90;
                toCheck = position + new Vector2Int(1, 0);
                return 0;
            case ExtendedTileType.DoorInner:
                Vector2Int toCheckD = position + new Vector2Int(0, -1);
                if ( tiles.ContainsKey(toCheckD) && tiles[toCheckD].type == Room.TileType.DOOR )
                    return 90;
                return 0;
        }
        return 0;
    }

    public static Vector2Int GetDirectionVector(GenTile.Position p) {
        switch ( p ) {
            case GenTile.Position.TOP:
                return new Vector2Int(0, 1);
            case GenTile.Position.LEFT:
                return new Vector2Int(-1, 0);
            case GenTile.Position.RIGHT:
                return new Vector2Int(1, 0);
            case GenTile.Position.BOTTOM:
                return new Vector2Int(0, -1);
            default:
                return new Vector2Int();
        }
    }

    private ExtendedTileType GetRandomGroundType() {
        int num = ( int ) ( UnityEngine.Random.value * 4 );
        switch ( num ) {
            case 0:
                return ExtendedTileType.Ground0;
            case 1:
                return ExtendedTileType.Ground1;
            case 2:
                return ExtendedTileType.Ground2;
            default:
                return ExtendedTileType.Ground3;
        }
    }
}
