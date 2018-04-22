using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour {
    private bool locked = false;

    [SerializeField]
    Room parent;

    BoxCollider2D boundingBox;
    BoxCollider2D triggerBox;

    // Use this for initialization
    void Start() {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach ( BoxCollider2D collider in colliders ) {
            if ( collider.isTrigger ) {
                triggerBox = collider;
                //Debug.Log("Found Door trigger");
            } else {
                boundingBox = collider;
                //Debug.Log("Found Door collider");
            }
        }
        Unlock();
    }

    /// <summary>
    /// Sets the parent Room Object reference.
    /// </summary>
    /// <param name="room"></param>
    public void SetParent(Room room) {
        this.parent = room;
    }

    /// <summary>
    /// Locks the door.
    /// </summary>
    public void Lock() {
        locked = true;
        boundingBox.enabled = true;
        triggerBox.enabled = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Unlocks the door.
    /// </summary>
    public void Unlock() {
        locked = false;
        boundingBox.enabled = false;
        triggerBox.enabled = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Returns if the door is locked.
    /// </summary>
    /// <returns></returns>
    public bool IsLocked() {
        return locked;
    }

    /// <summary>
    /// Check if a player moved inside of a room and notify the room.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision) {
        if ( collision.tag == "Player") {
            // TODO better checks
            Debug.Log("Leaving Trigger");
            if (parent != null) {
                parent.OnPlayerEnter(collision.gameObject.GetComponent<Player>());
            }
        }
    }
}
