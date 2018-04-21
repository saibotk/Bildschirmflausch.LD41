using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    int width, height;  // Gridsize for Generation

    List<Door> doors;
    List<Transform> spawnpoints;

    [SerializeField]
    GameObject doorsRootObject;

    [SerializeField]
    GameObject spawnpointRootObject;

    [SerializeField]
    private Objective objective; 

	// Use this for initialization
	void Start () {
        doors = new List<Door>();
        foreach (Door d in doorsRootObject.GetComponentsInChildren<Door>())
        {
            doors.Add(d);
        }
        Debug.Log("Doors in Room: " + doors.Count);
        spawnpoints = new List<Transform>();
        foreach (Transform t in spawnpointRootObject.GetComponentsInChildren<Transform>())
        {
            if( t.gameObject != spawnpointRootObject)
            {
                spawnpoints.Add(t);
            }
                
        }
        Debug.Log("Spawnpoints in Room: " + spawnpoints.Count);
    }
	
    public void SetObjective(Objective o)
    {
        objective = o;
    }

    public void Lock()
    {
        foreach (Door d in doors)
        {
            d.Lock();
        }
    }

    public void Unlock()
    {
        foreach (Door d in doors)
        {
            d.Unlock();
        }
    }

    public void OnPlayerEnter()
    {
        objective.Activate();
    }

    public List<Transform> GetSpawnpoints()
    {
        return spawnpoints;
    }
}
