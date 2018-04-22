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

    public GameObject ProcessRoom(Dictionary<Vector2, Room.TileType> d) {
        GameObject root = new GameObject {
            name = "Room"
        };
        foreach ( Vector2 v in d.Keys ) {
            bool left = false;
            bool top = false;
            bool right = false;
            bool bottom = false;
            // left bound
            if ( d.ContainsKey(v + new Vector2(-1, 0)) ) {
                if ( d[v + new Vector2(-1, 0)] == d[v] ) {
                    left = true;
                }
            }
            // top bound
            if ( d.ContainsKey(v + new Vector2(0, 1)) ) {
                if ( d[v + new Vector2(0, 1)] == d[v] ) {
                    top = true;
                }
            }
            // right bound
            if ( d.ContainsKey(v + new Vector2(1, 0)) ) {
                if ( d[v + new Vector2(1, 0)] == d[v] ) {
                    right = true;
                }
            }
            // bottom bound
            if ( d.ContainsKey(v + new Vector2(0, -1)) ) {
                if ( d[v + new Vector2(0, -1)] == d[v] ) {
                    bottom = true;
                }
            }
            ExtendedTileType type = ExtendedTileType.Ground;
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
            switch ( d[v] ) {
                case Room.TileType.WALL:
                    type = ExtendedTileType.BorderSingle;
                    if ( top && left && d.ContainsKey(v + new Vector2(-1, -1)) || top && right && d.ContainsKey(v + new Vector2(1, -1)) || right && bottom && d.ContainsKey(v + new Vector2(1, 1)) || left && bottom && d.ContainsKey(v + new Vector2(-1, 1)) ) {
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

            CreateGOFromType(v, type, root);
        }

        return root;
    }

    private GameObject CreateGOFromType(Vector2 v, ExtendedTileType t, GameObject root) {
        GameObject tmp = null;
        if ( prefabs.ContainsKey(t) && root != null ) {
            tmp = GameObject.Instantiate(prefabs[t], root.transform);
            tmp.transform.position = v;
        }
        return tmp;
    }
}
