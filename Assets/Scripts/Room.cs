using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    private bool activated;

    List<Door> doors;
    List<Transform> spawnpoints;

    [SerializeField]
    GameObject doorsRootObject;

    [SerializeField]
    GameObject spawnpointRootObject;

    [SerializeField]
    private Objective objective; 

    enum ObjectiveType { EntityObjective }
    // Params for testing
    [SerializeField]
    GameObject[] enemys;


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
        if (enemys.Length != 0)
            objective = new EntityObjective(this, new List<GameObject> (enemys));
        //Unlock();
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

    public Objective GetObjective()
    {
        return objective;
    }

    public void OnPlayerEnter()
    {
        if (activated)
            return;
        if(objective != null)
            objective.Activate();
        activated = true;
    }

    public List<Transform> GetSpawnpoints()
    {
        return spawnpoints;
    }

    public bool GetActivated()
    {
        return activated;
    }

}
