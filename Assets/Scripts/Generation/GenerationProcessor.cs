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

    public GameObject ProcessRoom(Dictionary<Vector2Int, Room.TileType> tiles) {
        GameObject root = new GameObject {
            name = "Room"
        };
		foreach ( Vector2Int v in tiles.Keys ) {
            bool left = false;
            bool top = false;
            bool right = false;
            bool bottom = false;
            // left bound
            if ( tiles.ContainsKey(v + new Vector2Int(-1, 0)) ) {
                if ( tiles[v + new Vector2Int(-1, 0)] == tiles[v] ) {
                    left = true;
                }
            }
            // top bound
            if ( tiles.ContainsKey(v + new Vector2Int(0, 1)) ) {
                if ( tiles[v + new Vector2Int(0, 1)] == tiles[v] ) {
                    top = true;
                }
            }
            // right bound
            if ( tiles.ContainsKey(v + new Vector2Int(1, 0)) ) {
                if ( tiles[v + new Vector2Int(1, 0)] == tiles[v] ) {
                    right = true;
                }
            }
            // bottom bound
            if ( tiles.ContainsKey(v + new Vector2Int(0, -1)) ) {
                if ( tiles[v + new Vector2Int(0, -1)] == tiles[v] ) {
                    bottom = true;
                }
            }
            ExtendedTileType type = ExtendedTileType.Ground;
			int rotation = 0;
            // ---------------------------------------------------------------------------------------------------------------------------------------------
            //  ^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~
            //  
            //                   ***        W A R N I N G    B A D    C O D E    A H E A D   !  !  !         ***
            //                      __________________________________________________________________________
            //
            //                                  DON'T WATCH, UNLESS YOU WANT TO GET TRAUMATIZED!
            //  
            //  ^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~^~!~
            // ---------------------------------------------------------------------------------------------------------------------------------------------
            switch ( tiles[v] ) {
                case Room.TileType.WALL:
                    type = ExtendedTileType.BorderSingle;
                    if ( top && left && tiles.ContainsKey(v + new Vector2Int(-1, -1)) 
					    || top && right && tiles.ContainsKey(v + new Vector2Int(1, -1)) 
					    || right && bottom && tiles.ContainsKey(v + new Vector2Int(1, 1))
					    || left && bottom && tiles.ContainsKey(v + new Vector2Int(-1, 1)) ) {
                        type = ExtendedTileType.BorderOuter;
                    } else if ( top && left || top && right || right && bottom || left && bottom ) {
                        type = ExtendedTileType.BorderInner;
                    }
                    break;
                case Room.TileType.GROUND:
                    type = ExtendedTileType.Ground;
                    break;
                case Room.TileType.DOOR:
                    type = ExtendedTileType.Door;
                    break;
                case Room.TileType.ROCK:
                    type = ExtendedTileType.Rock;
                    if ( top && !right && !left && !bottom ) {
                        type = ExtendedTileType.RockU;
                    }
                    if ( left && !right && !bottom && !top ) {
                        type = ExtendedTileType.RockL;
                    }
                    if ( right && !bottom && !left && !top ) {
                        type = ExtendedTileType.RockR;
                    }
                    if ( bottom && !right && !left && !top ) {
                        type = ExtendedTileType.RockD;
                    }
                    if ( left && top && !bottom && !right ) {
                        type = ExtendedTileType.RockLU;
                    }
                    if ( left && right && !top && !bottom ) {
                        type = ExtendedTileType.RockLR;
                    }
                    if ( left && bottom && !right && !top ) {
                        type = ExtendedTileType.RockLD;
                    }
                    if ( top && right && !left && !bottom ) {
                        type = ExtendedTileType.RockUR;
                    }
                    if ( top && bottom && !left && !right ) {
                        type = ExtendedTileType.RockUD;
                    }
                    if ( right && bottom && !top && !left ) {
                        type = ExtendedTileType.RockRD;
                    }

                    if ( left && top && bottom && !right ) {
                        type = ExtendedTileType.RockLUD;
                    }
                    if ( left && top && right && !bottom ) {
                        type = ExtendedTileType.RockLUR;
                    }
                    if ( top && right && bottom && !left ) {
                        type = ExtendedTileType.RockURD;
                    }
                    if ( left && right && bottom && !top ) {
                        type = ExtendedTileType.RockLRD;
                    }
                    if ( left && top && right && bottom ) {
                        type = ExtendedTileType.RockLURD;
                    }
                    break;
            }

            CreateGOFromType(v, rotation, type, root);
        }

        return root;
    }

    private GameObject CreateGOFromType(Vector2 v, int rotation, ExtendedTileType t, GameObject root) {
        GameObject tmp = null;
        if ( prefabs.ContainsKey(t) && root != null ) {
            tmp = Object.Instantiate(prefabs[t], root.transform);
            tmp.transform.position = v;
			tmp.transform.Rotate(new Vector3(0, 0, rotation));
        }
        return tmp;
    }
}
