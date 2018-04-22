using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob {

    public Player() : base(100) { }

    /// <summary>
    /// Collision checking. Player is going to die on any collision with a wall.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision");
        if ( collision.collider.tag == "wall" ) {
            Death();
        } else if ( collision.collider.tag == "enemy" ) {
            Mob m = collision.collider.GetComponent(typeof(Mob)) as Mob;
            if ( m != null ) {
                InflictDamage(m.GetDamage()); // TODO think about Mob attac mechanic
            }

        }
    }

    /// <summary>
    /// This is called when a Player died.
    /// </summary>
    protected override void Death() {
        Debug.Log("Player died...");
        Destroy(this.gameObject);
        GameController.instance.ChangeState(GameController.GameState.ENDED);
    }
}
