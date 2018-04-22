using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeBarController : MonoBehaviour {

    float currentRotation;
    int BrakeAmount = 100;
    private Player player;
    float firstTime;
    float secondTime;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // if player alive and spawned
        if (player != null)
        {
            UpdatePointer();
        }
        else if (currentRotation != 0)
        {
            //if player dead or not spawned
            UpdatePointer();
        }

    }
    private void UpdatePointer()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            firstTime = Time.time;
        }

            if (Input.GetKeyUp(KeyCode.S))
            {
                secondTime = Time.time;
                float difference = secondTime - firstTime;
                gameObject.transform.Rotate(Vector3.forward, difference);
            }
        
       
        }
    
    }

