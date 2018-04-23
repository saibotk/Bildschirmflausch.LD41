using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour {

    float currentRotation;
    float maxRotation;
    Player player;

    // Update is called once per frame
    void Update()
    {
        // if player alive and spawned
        if (player != null)
        {
            //Debug.Log(player.GetHealth());
            UpdatePointer(player.GetHealth());
        }
        else
        {
            //if player dead or not spawned
            UpdatePointer(0);
        }
    }

    void UpdatePointer(float playerLife) {
        float offset;

        if (player == null) {
            offset = -currentRotation;
            Debug.Log("Player not found");
        } else {
            //Debug.Log("calculated offset");
            offset = ((playerLife / maxRotation) * 100) - currentRotation;
        }
        offset /= 10;
        gameObject.transform.Rotate(Vector3.forward, offset);
        currentRotation += offset;
    }

    public void SetPlayer(Player ply) {
        player = ply;
        maxRotation = player.GetMaxHealth();
        //currentRotation = (player.GetHealth() / maxRotation) * 100;
        Debug.Log("Set Player");
    }
}
