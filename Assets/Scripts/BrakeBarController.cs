using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeBarController : MonoBehaviour {

    float currentRotation;
    int BrakeAmount = 100;
    private Player player;
    int currentBrakeAmount;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // if player alive and spawned
        if (player != null)
        {
            UpdatePointer(currentBrakeAmount);
        }
        else if (currentRotation != 0)
        {
            //if player dead or not spawned
            UpdatePointer(0);
        }

    }
    private void UpdatePointer(float brakeAmount)
    {
        if (Input.GetKey(KeyCode.S)) {
            float offset = brakeAmount - currentRotation;
            gameObject.transform.Rotate(Vector3.forward, offset);
            currentRotation += offset;
        }
    
    }
}
