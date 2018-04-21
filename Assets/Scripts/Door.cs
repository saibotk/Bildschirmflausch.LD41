using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool locked = false;

    BoxCollider2D boundingBox;
	// Use this for initialization
	void Start () {
        boundingBox = GetComponent<BoxCollider2D>();
        Unlock();
	}
	
    public void Lock()
    {
        locked = true;
        boundingBox.enabled = true;
    }

    public void Unlock()
    {
        locked = false;
        boundingBox.enabled = false;
    }

    public bool IsLocked()
    {
        return locked;
    }
}
