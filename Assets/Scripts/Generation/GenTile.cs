using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenTile
{
	public enum Position
	{
		TOP_LEFT = 0,
		TOP = 1,
		TOP_RIGHT = 2,
		LEFT = 3,
		CENTER = 4,
		RIGHT = 5,
		BOTTOM_LEFT = 6,
		BOTTOM = 7,
		BOTTOM_RIGHT = 8
	}

	public Room.TileType type;
	public Position position;

    public GenTile()
    {
		
	}

    public GenTile(Room.TileType type)
    {
        this.type = type;
	}

    public GenTile(Room.TileType type, Position position)
    {
        this.type = type;
		this.position = position;
    }

	public static Position GetPosition(int xMode, int yMode) {
		return (Position) ((xMode + 1) + (yMode + 1) * 3);
	}
}
