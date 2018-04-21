using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour{
	EntityObjective referringObjective;

	// Constructor
	public Entity(EntityObjective referringObjective)
	{
		this.referringObjective = referringObjective;
	}

	// kills the entity
	public void Kill()
	{
		if(referringObjective != null)
			referringObjective.Remove (this.gameObject);
	}
}
