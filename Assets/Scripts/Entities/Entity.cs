using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity {
	EntityObjective referringObjective;
	GameObject entityPrefab;
	GameObject instance;

	// Constructor
	public Entity(EntityObjective referringObjective, GameObject entityPrefab)
	{
		this.referringObjective = referringObjective;
		this.entityPrefab = entityPrefab;
	}

	// spawns the entity
	public void Spawn(Transform spawnPoint)
	{
		instance = GameObject.Instantiate (entityPrefab);
		instance.transform = spawnPoint;
	}

	// kills the entity
	public void Kill()
	{
		GameObject.Destroy (instance);
		referringObjective.Remove (this);

		instance = null;
	}
}
