using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    protected float speed = 1;
    [SerializeField]
    int damage = 0;

    GameObject owner;
	// Use this for initialization
	void Start () {
		
	}

    public void SetDamage(int dmg) {
        damage = dmg;
    }

    public void SetOwner(GameObject ow) {
        owner = ow;
    }

	// Update is called once per frame
	void Update () {
        if ( owner == null )
            return;
        transform.position = transform.position + transform.localRotation * Vector3.up * speed * Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if ( owner == null )
            return;
        Debug.Log("Collided");
        if ( collision.collider.gameObject.tag != owner.tag ) {
            Debug.Log("calc");
            Mob m = collision.collider.gameObject.GetComponent(typeof(Mob)) as Mob;
            if (m != null) {
                m.InflictDamage(damage);
            }
            Destroy(this.gameObject);
        }
        
    }
}
