using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool locked = false;

    BoxCollider2D boundingBox;
    BoxCollider2D triggerBox;
	// Use this for initialization
	void Start () {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders) {
            if (collider.isTrigger) {
                triggerBox = collider;
            } else {
                boundingBox = collider;
            }
        }
        Unlock();
	}
	
    public void Lock()
    {
        locked = true;
        boundingBox.enabled = true;
        triggerBox.enabled = false;
    }

    public void Unlock()
    {
        locked = false;
        boundingBox.enabled = false;
        triggerBox.enabled = true;
    }

    public bool IsLocked()
    {
        return locked;
    }
}
