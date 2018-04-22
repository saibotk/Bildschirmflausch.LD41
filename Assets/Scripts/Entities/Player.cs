using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob {

    public Player() : base(null, 100 )
    {
                
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        Debug.Log("Collision");
        if (collision.collider.tag == "wall") {
            Kill();
        } else if (collision.collider.tag == "enemy")
        {
            Mob m = collision.collider.GetComponent(typeof(Mob)) as Mob;
            if(m != null)
            {
                InflictDamage(m.GetDamage()); // TODO think about Mob attac mechanic
            }
            
        }
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag == "door") {
            //Debug.Log("Open door");
        }
	}

    public override void Kill()
    {
        base.Kill();
        Destroy(this.gameObject);
        GameController.instance.ChangeState(GameController.GameState.ENDED);
    } 
}
