using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour {

    float currentRotation;
    private Player player;

	// Use this for initialization
	void Start () {
        player = GameController.instance.GetPlayer();
        currentRotation = 100f;
	}
	
	// Update is called once per frame
	void Update () {
        // if player alive and spawned
        if (player != null) {
            gameObject.transform.Rotate(Vector3.forward, -(currentRotation - player.getHealth()));
            currentRotation = player.getHealth();
        } else {
            //if player dead or not spawned
            gameObject.transform.Rotate(Vector3.forward, -currentRotation);
            currentRotation = 0f;
            player = GameController.instance.GetPlayer();
        }
	}
}
