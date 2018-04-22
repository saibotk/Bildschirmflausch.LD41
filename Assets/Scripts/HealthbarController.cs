using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour {

    float currentRotation;
    float maxRotation;
    private Player player;

	// Use this for initialization
	void Start () {
        UpdatePlayer();
	}
	
	// Update is called once per frame
	void Update () {
        // if player alive and spawned
        if (player != null) {
            UpdatePointer(player.getHealth());
        } else {
            //if player dead or not spawned
            UpdatePointer(0);
            UpdatePlayer();

        }
	}

    private void UpdatePointer(float playerLife) {
        float offset = playerLife - currentRotation;
        gameObject.transform.Rotate(Vector3.forward, offset);
        currentRotation += offset;
    }

    private void UpdatePlayer() {
        player = GameController.instance.GetPlayer();
        if (player != null) {
            maxRotation = player.getMaxHealth();
            currentRotation = player.getHealth();
        }
    }
}
