using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenTile
{
	public enum Position
	{
		TOP, BOTTOM, LEFT, RIGHT, TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT
	}
	public Room.TileType type;
	public Position position;

    public GenTile()
    {
		
    }

	public GenTile(Room.TileType type) {
		this.type = type;
	}
}
