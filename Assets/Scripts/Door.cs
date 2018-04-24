using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour {
    private bool locked = false;
    private Animator animator;

    [SerializeField]
    Room parent;

    Vector2Int toOuter;

    BoxCollider2D boundingBox;
    BoxCollider2D triggerBox;

    // Use this for initialization
    void Awake() {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        animator = GetComponent<Animator>();
        foreach ( BoxCollider2D collider in colliders ) {
            if ( collider.isTrigger ) {
                triggerBox = collider;
                //Debug.Log("Found Door trigger");
            } else {
                boundingBox = collider;
                //Debug.Log("Found Door collider");
            }
        }
    }

    /// <summary>
    /// Sets the parent Room Object reference.
    /// </summary>
    /// <param name="room"></param>
    public void SetParent(Room room) {
        this.parent = room;
    }

    public void SetToOuter(Vector2Int v) {
        toOuter = v;
    }

    /// <summary>
    /// Locks the door.
    /// </summary>
    public void Lock() {
        animator.SetBool("open", false);
        locked = true;
        boundingBox.enabled = true;
        triggerBox.enabled = false;
        //GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Unlocks the door.
    /// </summary>
    public void Unlock() {
        animator.SetBool("open", true);
        locked = false;
        boundingBox.enabled = false;
        triggerBox.enabled = true;
        //GetComponent<SpriteRenderer>().enabled = false;
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
        // TODO only works correct for entering a room!
        if ( collision.tag == "Player") {
			Player player = collision.gameObject.GetComponent<Player>();
            Vector2 centerToCollider = (Vector2) gameObject.transform.position - parent.GetPosition() + parent.GetCenter();
            Vector2 centerToPlayer = (Vector2) player.gameObject.transform.position - parent.GetPosition() + parent.GetCenter();
            float angle = Vector2.Angle(( centerToPlayer - centerToCollider ), toOuter);
            if ( (angle > 90 && angle < 270) ) {
                Debug.Log("Player is on the outside! Angle: " + angle);
                return;
            }
            Debug.Log("Leaving Trigger");
            if(parent == null) {
                Debug.Log("This door has no parent Room!");
                return;
            }
            parent.OnPlayerEnter(player);
        }
    }
}
