using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityObjective : Objective{
	List<Entity> entityList;
	List<Transform> spawnPointList;

	// Constructor
	public EntityObjective(Room objectiveCaller, List<Entity> entityList) : base(objectiveCaller)
	{
		this.entityList = entityList;
		spawnPointList = objectiveCaller.GetSpawnpoints ();
	}

	// Activates the objective to start progresstracking
	public void Activate()
	{
		Random newRand = new Random ();

		foreach (Entity i in entityList)
			i.Spawn(spawnPointList[Random.Range(0, spawnPointList.Count)]);

		objectiveCaller.Lock();
	}

	// Removes the entity from the list and completes the objective, if the list is empty
	public void Remove(Entity inputEntity)
	{
		entityList.Remove (inputEntity);
		if (entityList.Count == 0)
			objectiveCaller.Unlock ();
	}
}