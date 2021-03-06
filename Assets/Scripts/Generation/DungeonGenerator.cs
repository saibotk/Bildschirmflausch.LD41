﻿using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator {
    public DungeonGenerator() {
    }

    public const int TUNNEL_THICKNESS = 4;

    // The first and starting room
    public GenRoom start;
    // The room with the finishing flag
    public GenRoom end;
    // The room containing all the paths connecting normal rooms
    public GenRoom path;
    // All rooms except the three above
    public HashSet<GenRoom> rooms;

    public void Generate() {
        int minRoomSize = 50;
        rooms = new HashSet<GenRoom>();
        for ( int i = 0; i < 7 + ( int ) ( UnityEngine.Random.value * 4 ); i++ ) {
            GenRoom room = new GenRoom();
            room.bounds.width = ( ( 15 + ( int ) ( UnityEngine.Random.value * 20 ) ) / 2 ) * 2;
            room.bounds.height = ( ( 15 + ( int ) ( UnityEngine.Random.value * 20 ) ) / 2 ) * 2;
            rooms.Add(room);
        }

        while ( true ) {
            bool changed = false;
            foreach ( GenRoom r1 in rooms ) {
                foreach ( GenRoom r2 in rooms ) {
                    if ( r1 == r2 )
                        continue;
                    Vector2Int p1 = new Vector2Int(r1.bounds.x + r1.bounds.width / 2, r1.bounds.y + r1.bounds.height / 2);
                    Vector2Int p2 = new Vector2Int(r2.bounds.x + r2.bounds.width / 2, r2.bounds.y + r2.bounds.height / 2);
                    if ( Math.Pow(Vector2Int.Distance(p1, p2), 2) < 2 * minRoomSize * minRoomSize + 2 ) {
                        r2.bounds.x += ( int ) ( ( UnityEngine.Random.value - 0.5 ) * 5 );
                        r2.bounds.y += ( int ) ( ( UnityEngine.Random.value - 0.5 ) * 2.5 );
                        changed = true;
                        break;
                    }
                }
                if ( changed )
                    break;
            }
            if ( !changed )
                break;
        }

        HashSet<GenVertex> Q = new HashSet<GenVertex>();
        foreach ( GenRoom r in rooms )
            Q.Add(new GenVertex(r));
        GenVertex root = null;
        foreach ( GenVertex v in Q ) {
            if ( root == null || v.r.bounds.x < root.r.bounds.x )
                root = v;
        }
        root.value = 0;

        HashSet<GenEdge> E = new HashSet<GenEdge>();
        HashSet<GenEdge> G = new HashSet<GenEdge>();
        HashSet<GenVertex> F = new HashSet<GenVertex>();
        foreach ( GenVertex r1 in Q ) {
            foreach ( GenVertex r2 in Q ) {
                if ( r1 == r2 )
                    goto outer;
                foreach ( GenEdge e in E )
                    if ( e.r2 == r1 && e.r1 == r2 )
                        goto outer;
                E.Add(new GenEdge(r1, r2));
            }
        outer:;
        }
        F.Add(root);
        Q.Remove(root);

        while ( Q.Count > 0 ) {
            GenEdge start2 = null;
            foreach ( GenEdge e in E ) {
                if ( F.Contains(e.r1) ^ F.Contains(e.r2) ) {
                    if ( start2 == null || e.dist < start2.dist ) {
                        start2 = e;
                    }
                }
            }
            Q.Remove(start2.r2);
            Q.Remove(start2.r1);
            F.Add(start2.r2);
            F.Add(start2.r1);
            E.Remove(start2);
            G.Add(start2);
            if ( start2.r1.value < start2.r2.value ) {
                start2.r2.value = ( float ) ( start2.r1.value + start2.dist );
            } else {
                start2.r1.value = ( float ) ( start2.r2.value + start2.dist );
            }
        }

        // G list of edges
        // rooms list of rooms

        HashSet<GenRoom> rooms2 = new HashSet<GenRoom>();

        foreach ( GenEdge ed in G ) {
            // horizontal
            float diff1 = ed.r1.r.bounds.y - ed.r2.r.bounds.y - ed.r2.r.bounds.height + TUNNEL_THICKNESS;
            float diff2 = ed.r2.r.bounds.y - ed.r1.r.bounds.y - ed.r1.r.bounds.height + TUNNEL_THICKNESS;

            // vertical
            float diff3 = ed.r1.r.bounds.x - ed.r2.r.bounds.x - ed.r2.r.bounds.width + TUNNEL_THICKNESS;
            float diff4 = ed.r2.r.bounds.x - ed.r1.r.bounds.x - ed.r1.r.bounds.width + TUNNEL_THICKNESS;

            if ( diff1 < 0 && diff2 < 0 ) {
                AddStraightHorizontal(rooms2, ed);
            } else if ( diff3 < 0 && diff4 < 0 ) {
                AddStraightVertical(rooms2, ed);
            } else
                AddCurve(rooms2, ed);
        }

		HashSet<Vector2Int> allDoors = new HashSet<Vector2Int>();
        foreach ( GenRoom r in rooms ) {
            for ( int x1 = r.bounds.x; x1 < r.bounds.x + r.bounds.width; x1++ )
                for ( int y1 = r.bounds.y; y1 < r.bounds.y + r.bounds.height; y1++ ) {
                    int xMode = (x1 == r.bounds.x) ? -1 : (x1 == r.bounds.x + r.bounds.width - 1) ? 1 : 0;
                    int yMode = (y1 == r.bounds.y) ? -1 : (y1 == r.bounds.y + r.bounds.height - 1) ? 1 : 0;
					r.tiles.Add(new Vector2Int(x1, y1), new GenTile(Room.TileType.WALL, GenTile.GetPosition(xMode,yMode)));
                }
            for ( int x1 = r.bounds.x + 1; x1 < r.bounds.x + r.bounds.width - 1; x1++ )
                for ( int y1 = r.bounds.y + 1; y1 < r.bounds.y + r.bounds.height - 1; y1++ ) {
					r.tiles[new Vector2Int(x1, y1)].type = Room.TileType.GROUND;
                }
			allDoors.UnionWith(r.AllDoors());
			foreach (Vector2Int v in r.doorsDown) {
                r.tiles[v].type = Room.TileType.DOOR;
				r.tiles[v].position = GenTile.Position.BOTTOM;
			}
			foreach (Vector2Int v in r.doorsUp) {
                r.tiles[v].type = Room.TileType.DOOR;
				r.tiles[v].position = GenTile.Position.TOP;
			}
			foreach (Vector2Int v in r.doorsLeft) {
                r.tiles[v].type = Room.TileType.DOOR;
				r.tiles[v].position = GenTile.Position.LEFT;
			}
			foreach (Vector2Int v in r.doorsRight) {
                r.tiles[v].type = Room.TileType.DOOR;
				r.tiles[v].position = GenTile.Position.RIGHT;
            }
		}

        path = new GenRoom();
        foreach (GenRoom r in rooms2)
		{
            for (int x1 = r.bounds.x; x1 < r.bounds.x + r.bounds.width; x1++)
                for (int y1 = r.bounds.y; y1 < r.bounds.y + r.bounds.height; y1++)
                {
                    Vector2Int pos1 = new Vector2Int(x1, y1);
                    if (path.tiles.ContainsKey(pos1))
                        path.tiles[pos1].type = Room.TileType.GROUND;
                    else
                        path.tiles.Add(pos1, new GenTile(Room.TileType.GROUND));

                    Vector2Int pos2 = new Vector2Int(x1 + 1, y1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.RIGHT));
                    pos2 = new Vector2Int(x1 - 1, y1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.LEFT));
                    pos2 = new Vector2Int(x1, y1 + 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.TOP));
                    pos2 = new Vector2Int(x1, y1 - 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.BOTTOM));
			}
            for (int x1 = r.bounds.x; x1 < r.bounds.x + r.bounds.width; x1++)
                for (int y1 = r.bounds.y; y1 < r.bounds.y + r.bounds.height; y1++)
                {
                    Vector2Int pos2 = new Vector2Int(x1 + 1, y1 + 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.TOP_RIGHT));
                    pos2 = new Vector2Int(x1 - 1, y1 + 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.TOP_LEFT));
                    pos2 = new Vector2Int(x1 + 1, y1 - 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
					    path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.BOTTOM_RIGHT));
                    pos2 = new Vector2Int(x1 - 1, y1 - 1);
                    if (!path.tiles.ContainsKey(pos2) && !allDoors.Contains(pos2))
                        path.tiles.Add(pos2, new GenTile(Room.TileType.WALL, GenTile.Position.BOTTOM_LEFT));
                }
            if (r.AllDoors().Count > 0)
                throw new NotSupportedException("Paths should not have any doors");
        }

        start = root.r;
        end = null; 
		foreach ( GenRoom r in rooms ) {
            if ( end == null || r.bounds.x > end.bounds.x )
				end = r;
        }

        rooms.Remove(start);
		rooms.Remove(end);

        foreach (GenRoom r in rooms)
        {
            GenerateInterior(r);
        }

		start.spawnpoints.Add(start.GetCenter());
        end.spawnpoints.Add(end.GetCenter());

		foreach (Vector2Int v in allDoors)
		{
			foreach (GenRoom r in rooms)
			{
				for (int x = -TUNNEL_THICKNESS; x < TUNNEL_THICKNESS; x++)
					for (int y = -TUNNEL_THICKNESS; y < TUNNEL_THICKNESS; y++)
						if (r.tiles.ContainsKey(v + new Vector2Int(x, y)) && r.tiles[v + new Vector2Int(x, y)].type == Room.TileType.ROCK)
							r.tiles[v + new Vector2Int(x, y)].type = Room.TileType.GROUND;
			}
			{
				GenRoom r = path;
				if (r.tiles.ContainsKey(v + new Vector2Int(0, 1)) && r.tiles[v + new Vector2Int(0, 1)].type == Room.TileType.WALL)
					r.tiles.Remove(v + new Vector2Int(0, 1));
				if (r.tiles.ContainsKey(v + new Vector2Int(0, -1)) && r.tiles[v + new Vector2Int(0, -1)].type == Room.TileType.WALL)
					r.tiles.Remove(v + new Vector2Int(0, -1));
				if (r.tiles.ContainsKey(v + new Vector2Int(1, 0)) && r.tiles[v + new Vector2Int(1, 0)].type == Room.TileType.WALL)
					r.tiles.Remove(v + new Vector2Int(1, 0));
				if (r.tiles.ContainsKey(v + new Vector2Int(-1, 0)) && r.tiles[v + new Vector2Int(-1, 0)].type == Room.TileType.WALL)
					r.tiles.Remove(v + new Vector2Int(-1, 0));
			}
		}

        foreach ( GenRoom r in rooms )
            MakeRoomRelative(r);
        MakeRoomRelative(start);
        MakeRoomRelative(end);
        MakeRoomRelative(path);
    }

    public void MakeRoomRelative(GenRoom room) {
        room.roomPosition = room.bounds.position;
        foreach ( Vector2Int v in room.tiles.Keys ) {
            v.Set(( v - room.roomPosition ).x, ( v - room.roomPosition ).y);
        }
    }

    public static void AddStraightHorizontal(HashSet<GenRoom> rooms, GenEdge ed) {
        GenRoom righter = ed.r1.r.GetCenter().x > ed.r2.r.GetCenter().x ? ed.r1.r : ed.r2.r;
        GenRoom lefter = ed.r1.r.GetCenter().x > ed.r2.r.GetCenter().x ? ed.r2.r : ed.r1.r;
        GenRoom tunnel = new GenRoom();
        int minX = Math.Min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
        int minY = Math.Max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
        int maxX = Math.Max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
        int maxY = Math.Min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
        tunnel.bounds.x = minX;
        tunnel.bounds.y = ( minY + maxY ) / 2 - TUNNEL_THICKNESS / 2;
        tunnel.bounds.width = ( maxX - minX );
        tunnel.bounds.height = TUNNEL_THICKNESS;

        rooms.Add(tunnel);

        for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
            lefter.doorsRight.Add(new Vector2Int(tunnel.bounds.x - 1, tunnel.bounds.y + i));
            righter.doorsLeft.Add(new Vector2Int(tunnel.bounds.x + tunnel.bounds.width, tunnel.bounds.y + i));
        }
    }

    public static void AddStraightVertical(HashSet<GenRoom> rooms, GenEdge ed) {
        GenRoom higher = ed.r1.r.GetCenter().y > ed.r2.r.GetCenter().y ? ed.r1.r : ed.r2.r;
        GenRoom lower = ed.r1.r.GetCenter().y > ed.r2.r.GetCenter().y ? ed.r2.r : ed.r1.r;
        GenRoom tunnel = new GenRoom();
        int minX = Math.Max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
        int minY = Math.Min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
        int maxX = Math.Min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
        int maxY = Math.Max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
        tunnel.bounds.x = ( minX + maxX ) / 2 - TUNNEL_THICKNESS / 2;
        tunnel.bounds.y = minY;
        tunnel.bounds.width = TUNNEL_THICKNESS;
        tunnel.bounds.height = ( maxY - minY );

        rooms.Add(tunnel);

        for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
            higher.doorsDown.Add(new Vector2Int(tunnel.bounds.x + i, tunnel.bounds.y + tunnel.bounds.height));
            lower.doorsUp.Add(new Vector2Int(tunnel.bounds.x + i, tunnel.bounds.y - 1));
        }
    }

    public static void AddCurve(HashSet<GenRoom> rooms, GenEdge ed) {
        GenRoom higher = ed.r1.r.GetCenter().y > ed.r2.r.GetCenter().y ? ed.r1.r : ed.r2.r;
        GenRoom lower = ed.r1.r.GetCenter().y > ed.r2.r.GetCenter().y ? ed.r2.r : ed.r1.r;
        GenRoom righter = ed.r1.r.GetCenter().x > ed.r2.r.GetCenter().x ? ed.r1.r : ed.r2.r;
        GenRoom lefter = ed.r1.r.GetCenter().x > ed.r2.r.GetCenter().x ? ed.r2.r : ed.r1.r;

        RectInt r = new RectInt(lefter.GetCenter().x, lower.GetCenter().y, righter.GetCenter().x - lefter.GetCenter().x, higher.GetCenter().y - lower.GetCenter().y);

        GenRoom verticalLefter = new GenRoom();
        verticalLefter.bounds.x = r.x - TUNNEL_THICKNESS / 2;
        verticalLefter.bounds.y = r.y - TUNNEL_THICKNESS / 2;
        verticalLefter.bounds.width = TUNNEL_THICKNESS;
        verticalLefter.bounds.height = r.height + TUNNEL_THICKNESS;

        GenRoom horizontalLower = new GenRoom();
        horizontalLower.bounds.x = r.x - TUNNEL_THICKNESS / 2;
        horizontalLower.bounds.y = r.y - TUNNEL_THICKNESS / 2;
        horizontalLower.bounds.width = r.width + TUNNEL_THICKNESS;
        horizontalLower.bounds.height = TUNNEL_THICKNESS;

        GenRoom verticalRighter = new GenRoom();
        verticalRighter.bounds.x = r.x + r.width - TUNNEL_THICKNESS / 2;
        verticalRighter.bounds.y = r.y - TUNNEL_THICKNESS / 2;
        verticalRighter.bounds.width = TUNNEL_THICKNESS;
        verticalRighter.bounds.height = r.height + TUNNEL_THICKNESS;

        GenRoom horizontalHigher = new GenRoom();
        horizontalHigher.bounds.x = r.x - TUNNEL_THICKNESS / 2;
        horizontalHigher.bounds.y = r.y + r.height - TUNNEL_THICKNESS / 2;
        horizontalHigher.bounds.width = r.width + TUNNEL_THICKNESS;
        horizontalHigher.bounds.height = TUNNEL_THICKNESS;

        if ( lower == lefter ) {
            horizontalLower.bounds.x = r.x + lower.bounds.width / 2;
            horizontalLower.bounds.width = r.width - lower.bounds.width / 2 + TUNNEL_THICKNESS / 2;
            horizontalHigher.bounds.width = r.width - higher.bounds.width / 2 + TUNNEL_THICKNESS / 2;

            verticalLefter.bounds.y = r.y + lower.bounds.height / 2;
            verticalLefter.bounds.height = r.height - lower.bounds.height / 2 + TUNNEL_THICKNESS / 2;
            verticalRighter.bounds.height = r.height - higher.bounds.height / 2 + TUNNEL_THICKNESS / 2;
        }
        if ( lower == righter ) {
            horizontalHigher.bounds.x = r.x + higher.bounds.width / 2;
            horizontalHigher.bounds.width = r.width - higher.bounds.width / 2 + TUNNEL_THICKNESS / 2;
            horizontalLower.bounds.width = r.width - lower.bounds.width / 2 + TUNNEL_THICKNESS / 2;

            verticalRighter.bounds.y = r.y + lower.bounds.height / 2;
            verticalRighter.bounds.height = r.height - lower.bounds.height / 2 + TUNNEL_THICKNESS / 2;
            verticalLefter.bounds.height = r.height - higher.bounds.height / 2 + TUNNEL_THICKNESS / 2;
        }

        bool flip = UnityEngine.Random.value > 0.5;
        bool diffX = ed.r2.r.GetCenter().x - ed.r1.r.GetCenter().x > 0;
        bool diffY = ed.r2.r.GetCenter().y - ed.r1.r.GetCenter().y > 0;
        bool addHorizontal1 = false, addHorizontal2 = false, addVertical1 = false, addVertical2 = false;
        if ( diffX && diffY ) {
            if ( flip ) {
                addVertical1 = true;
                addHorizontal2 = true;
            } else {
                addVertical2 = true;
                addHorizontal1 = true;
            }
        } else if ( diffX && !diffY ) {
            if ( flip ) {
                addVertical2 = true;
                addHorizontal2 = true;
            } else {
                addVertical1 = true;
                addHorizontal1 = true;
            }
        } else if ( !diffX && diffY ) {
            if ( flip ) {
                addVertical1 = true;
                addHorizontal1 = true;
            } else {
                addVertical2 = true;
                addHorizontal2 = true;
            }
        } else if ( !diffX && !diffY ) {
            if ( flip ) {
                addVertical2 = true;
                addHorizontal1 = true;
            } else {
                addVertical1 = true;
                addHorizontal2 = true;
            }
        }
        if ( addHorizontal1 ) {
            rooms.Add(horizontalLower);
            if ( lower == lefter )
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    lower.doorsRight.Add(new Vector2Int(horizontalLower.bounds.x - 1, horizontalLower.bounds.y + i));
                }
			else
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    lower.doorsLeft.Add(new Vector2Int(horizontalLower.bounds.x + horizontalLower.bounds.width, horizontalLower.bounds.y + i));
                }
        }
        if ( addHorizontal2 ) {
            rooms.Add(horizontalHigher);
            if ( lower == righter )
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    higher.doorsRight.Add(new Vector2Int(horizontalHigher.bounds.x - 1, horizontalHigher.bounds.y + i));
                }
			else
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    higher.doorsLeft.Add(new Vector2Int(horizontalHigher.bounds.x + horizontalHigher.bounds.width, horizontalHigher.bounds.y + i));
                }
        }
        if ( addVertical1 ) {
            rooms.Add(verticalLefter);
            if ( lower == lefter )
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    lower.doorsUp.Add(new Vector2Int(verticalLefter.bounds.x + i, verticalLefter.bounds.y - 1));
                }
			else
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    higher.doorsDown.Add(new Vector2Int(verticalLefter.bounds.x + i, verticalLefter.bounds.y + verticalLefter.bounds.height));
                }
        }
        if ( addVertical2 ) {
            rooms.Add(verticalRighter);
            if ( lower == righter )
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    lower.doorsUp.Add(new Vector2Int(verticalRighter.bounds.x + i, verticalRighter.bounds.y - 1));
                }
			else
                for ( int i = 0; i < TUNNEL_THICKNESS; i++ ) {
                    higher.doorsDown.Add(new Vector2Int(verticalRighter.bounds.x + i, verticalRighter.bounds.y + verticalRighter.bounds.height));
                }
        }
    }

    public void GenerateInterior(GenRoom r) {
        Vector2Int root = new Vector2Int (1, 1);

		for (int x = r.bounds.x; x < r.bounds.x + r.bounds.width; x++)
		{
			for (int y = r.bounds.y; y < r.bounds.y + r.bounds.height; y++)
			{
				Vector2Int pos = new Vector2Int(x, y);
				if (!r.tiles.ContainsKey(pos) || r.tiles[pos].type != Room.TileType.GROUND)
					continue;
				float prob = 0.0075f;
				if (UnityEngine.Random.value > 1 - prob * 2)
                {
					int count = (int ) (UnityEngine.Random.value * UnityEngine.Random.value * 6);
					for (int i = 0; i < count; i++) {
						Vector2Int pos2 = pos + new Vector2Int(
							(int)((UnityEngine.Random.value - 0.5) * 3), 
							(int)((UnityEngine.Random.value - 0.5) * 3));
						if (r.tiles.ContainsKey(pos2) && r.tiles[pos2].type == Room.TileType.GROUND)
    						r.tiles[pos2].type = Room.TileType.ROCK;
                    }
					continue;
				}
                if (UnityEngine.Random.value > 1 - prob)
                {
                    r.tiles[pos].type = Room.TileType.ROCK;
					continue;
                }
				float prob2 = 0.04f;
				if (UnityEngine.Random.value > 1 - prob2 
				    && x > r.bounds.x + 2
				    && x < r.bounds.x + r.bounds.width-2
				    && y > r.bounds.y+ 2
				    && y < r.bounds.y + r.bounds.height-2)
				{
					r.spawnpoints.Add(pos);
				}
			}
		}
		//Debug.Log(r.GetCenter() + " " + r.spawnpoints.Count + " spawn points.");
    }

    public static void GenerateObjective(Room r) {
        Dictionary<Entity.Entities, GameObject> enemyPrefabs = GameController.instance.GetEntitiesPrefabs(); // TODO not so cool
        
        Objective o = null;

        int rand = UnityEngine.Random.Range(0, 8);
		switch (rand) {
            case 0:
            case 7:
                {
                    // Scorpion Attack!!!!:
                    List<GameObject> lg = new List<GameObject>();
					int count = UnityEngine.Random.Range(1, ( int ) Math.Min(Mathf.Floor(r.GetSpawnpoints().Count), 4));
                    for ( int c = 0; c < count; c++ ) {
                        lg.Add(enemyPrefabs[Entity.Entities.SCORPION]);
                    }
                    EntityObjective eo = new EntityObjective(r, lg);
                    o = eo;
                }
				break;
			case 2:
            case 3: {
                    // Bug Attack!!!!:
                    List<GameObject> lg = new List<GameObject>();
					int count = Math.Min(UnityEngine.Random.Range(1, ( int ) Mathf.Floor(r.GetSpawnpoints().Count)), 7);
                    for ( int c = 0; c < count; c++ ) {
                        lg.Add(enemyPrefabs[Entity.Entities.BUG]);
                    }
                    EntityObjective eo = new EntityObjective(r, lg);
                    o = eo;
                }
				break;
			case 4:
            case 5:
                {
                    // Coins!!!!:
                    List<GameObject> lg = new List<GameObject>();
                    int count = Math.Min(UnityEngine.Random.Range(1, (int)Mathf.Floor(r.GetSpawnpoints().Count)), 20);
                    for (int c = 0; c < count; c++)
                    {
                        lg.Add(enemyPrefabs[Entity.Entities.COIN]);
                    }
                    EntityObjective eo = new EntityObjective(r, lg);
                    o = eo;
				}
                break;
			case 6:
            case 1:
                {
                    // Siders!!!!:
                    List<GameObject> lg = new List<GameObject>();
                    int count = Math.Min(UnityEngine.Random.Range(1, (int)Mathf.Floor(r.GetSpawnpoints().Count)), 5);
                    for (int c = 0; c < count; c++)
                    {
						lg.Add(enemyPrefabs[Entity.Entities.SPIDER]);
                    }
                    EntityObjective eo = new EntityObjective(r, lg);
                    o = eo;
                }
            break;
        }
        
        r.SetObjective(o);
    }
}