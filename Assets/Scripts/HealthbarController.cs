using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour {

    float currentRotation;
    float maxRotation;
    private Player player;

    // Update is called once per frame
    void Update() {
        // if player alive and spawned
        if ( player != null ) {
            UpdatePointer(player.GetHealth());
        } else if (currentRotation != 0) {
            //if player dead or not spawned
            UpdatePointer(0);
        }
    }

    private void UpdatePointer(float playerLife) {
        float offset = playerLife - currentRotation;
        gameObject.transform.Rotate(Vector3.forward, offset);
        currentRotation += offset;
    }

    public void SetPlayer(Player ply) {
        player = ply;
    }
}
