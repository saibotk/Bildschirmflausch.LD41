using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool locked = false;

    [SerializeField]
    GameObject graphics;

    [SerializeField]
    Room parent; 

    BoxCollider2D boundingBox;
    BoxCollider2D triggerBox;
	// Use this for initialization
	void Start () {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders) {
            if (collider.isTrigger) {
                triggerBox = collider;
                Debug.Log("Found Door trigger");
            } else {
                boundingBox = collider;
                Debug.Log("Found Door collider");
            }
        }
        Unlock();
	}
	
    public void Lock()
    {
        locked = true;
        boundingBox.enabled = true;
        triggerBox.enabled = false;
        graphics.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Unlock()
    {
        locked = false;
        boundingBox.enabled = false;
        triggerBox.enabled = true;
        graphics.GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool IsLocked()
    {
        return locked;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Leavin Trigger");
            parent.OnPlayerEnter();
        }
    }
}
