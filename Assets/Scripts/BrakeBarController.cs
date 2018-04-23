using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeBarController : MonoBehaviour {

    float maxRotation = 129;
    float currentRotation = 0;
    private Player player;

    // Update is called once per frame
    void Update() {
        float offset = 0;
        // if player alive and spawned
        if (player != null) {
            offset = CalculateOffset();
        }
        else {
            offset = -currentRotation;
        }
        currentRotation += offset;
        gameObject.transform.Rotate(Vector3.forward, -offset);
    }

    private float CalculateOffset() {
		return (maxRotation * (player.GetComponent<PlayerMovement>().GetBrakeTime() / player.GetComponent<PlayerMovement>().maxBrakeTime)) - currentRotation;
    }

    public void SetPlayer(Player ply) {
        player = ply;
        maxRotation = 129;
        currentRotation = 0;
    }
}
