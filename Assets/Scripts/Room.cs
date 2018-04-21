using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    int Width, Height;  // Gridsize for Generation
    List<Door> doors;

    [SerializeField]
    GameObject doorsRootObject;

    [SerializeField]
    private Objective objective; 

	// Use this for initialization
	void Start () {
        doors = new List<Door>();
        foreach (Door d in doorsRootObject.GetComponentsInChildren<Door>())
        {
            doors.Add(d);
        }
	}
	

    void Lock()
    {
        foreach (Door d in doors)
        {
            d.Lock();
        }
    }

    void Unlock()
    {
        foreach (Door d in doors)
        {
            d.Unlock();
        }
    }


}
