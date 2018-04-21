using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob {

    public Player() : base(null, 100 )
    {
                
    }

	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log("Collision");
        if (collision.collider.tag == "wall") {
            Kill();
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "door") {
            Debug.Log("Open door");
        }
	}

    public override void Kill()
    {
        base.Kill();
        Destroy(this.gameObject);
    } 
}
