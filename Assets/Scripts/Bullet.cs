using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    protected float speed = 1;
    [SerializeField]
    int damage = 0;
    [SerializeField]
    bool start = false;
    string ownertag;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    public void StartBullet() {
        start = true;
    }

    public void SetSpeed(float spd) {
        speed = spd;
    }

    public void SetDamage(int dmg) {
        damage = dmg;
    }

    public void SetOwner(GameObject ow) {
        ownertag = ow.tag;
    }

    private void FixedUpdate() {
        if ( start == false )
            return;
        body.MovePosition(transform.position + transform.localRotation * Vector3.up * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if ( ownertag == null )
            return;

        if ( collider.gameObject.tag != ownertag ) {
            Mob m = collider.gameObject.GetComponent(typeof(Mob)) as Mob;
            if (m != null) {
                m.InflictDamage(damage);
            }
            Destroy(gameObject);
        }

    }
}
