using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityObjective : Objective {
	List<GameObject> prefabList;
	List<GameObject> entityList;
	List<Transform> spawnPointList;

	// Constructor
	public EntityObjective(Room objectiveCaller, List<GameObject> prefabList) : base(objectiveCaller)
	{
        this.entityList = new List<GameObject>();
		this.prefabList = prefabList;
		spawnPointList = objectiveCaller.GetSpawnpoints ();
	}

	// Activates the objective to start progresstracking
	public override void Activate()
	{
		Random newRand = new Random ();
        Debug.Log("Activate");
		foreach (GameObject i in prefabList) 
		{
            Debug.Log("Instantiating Prefab");
			GameObject tempObject = GameObject.Instantiate (i);
			tempObject.transform.position = spawnPointList [Random.Range (0, spawnPointList.Count)].position;
            entityList.Add(tempObject);
		}

		objectiveCaller.Lock();
	}

	// Removes the entity from the list and completes the objective, if the list is empty
	public void Remove(GameObject inputEntity)
	{
		entityList.Remove (inputEntity);
		if (entityList.Count == 0)
			objectiveCaller.Unlock ();
	}

    public List<GameObject> GetEntities()
    {
        return entityList;
    }
}