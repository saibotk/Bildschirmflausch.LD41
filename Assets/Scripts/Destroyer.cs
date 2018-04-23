using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(ps != null && !ps.isPlaying) {
            Destroy(this.gameObject);
        }
	}
}
