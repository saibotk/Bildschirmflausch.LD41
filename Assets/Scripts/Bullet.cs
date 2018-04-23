using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    protected float speed = 1;
    [SerializeField]
    int damage = 0;

    GameObject owner;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

    }

    public void SetDamage(int dmg) {
        damage = dmg;
    }

    public void SetOwner(GameObject ow) {
        owner = ow;
    }

    private void FixedUpdate() {
        if ( owner == null )
            return;
        body.MovePosition(transform.position + transform.localRotation * Vector3.up * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if ( owner == null )
            return;
        
        if ( collider.gameObject.tag != owner.tag ) {
            Mob m = collider.gameObject.GetComponent(typeof(Mob)) as Mob;
            if (m != null) {
                m.InflictDamage(damage);
            }
            Destroy(this.gameObject);
        }
        
    }
}
