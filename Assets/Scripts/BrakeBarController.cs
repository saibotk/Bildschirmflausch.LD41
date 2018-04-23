using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeBarController : MonoBehaviour {


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
        
        }

    
    private void UpdatePointer()
    {
        while (Input.GetKey(KeyCode.S))
        {
            firstTime += 1;
        }

            if (Input.GetKeyUp(KeyCode.S))
            {
                
                gameObject.transform.Rotate(Vector3.forward, firstTime);
            }
        
       
        }
    
    }

