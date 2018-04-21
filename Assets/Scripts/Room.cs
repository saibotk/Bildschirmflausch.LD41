using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    int Width, Height;  // Gridsize for Generation

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
        spawnpoints = new List<Transform>();
        foreach (Transform t in spawnpointRootObject.GetComponentsInChildren<Transform>())
        {
            spawnpoints.Add(t);
        }
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

    public List<Transform> GetSpawnpoints()
    {
        return spawnpoints;
    }
}
