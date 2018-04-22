using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	private GameObject victim;
    Rigidbody2D body;
	[SerializeField]
	private float speed = 1;
	[SerializeField]
	private float rotationSpeed = 1;

	/*
	 * Die destructive dumme deutsche Dungeon Diktator Drifter DLC Debakel Distribution Dokumentations - Druck Datei
	 */

	// Use this for initialization
	void Start () {
        victim = null;
        body = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update ()
	{
        if(victim == null) {
            victim = GameController.instance.GetPlayer().gameObject; // TODO testing purpose only!
            return;
        }

		Vector3 distanceToEnemy = victim.transform.position - gameObject.transform.position;

        // movement
        body.velocity = new Vector2(distanceToEnemy.normalized.x, distanceToEnemy.normalized.y) * speed;

        //rotation
        Vector3 localRotation = gameObject.transform.localRotation * Vector3.up;
        float angleToRotate = Mathf.Round(Vector3.SignedAngle(localRotation, distanceToEnemy.normalized, Vector3.forward));
        gameObject.transform.Rotate(0, 0, angleToRotate * rotationSpeed);
	}

    public void SetVictim(GameObject g) {
        victim = g;
    }
}
