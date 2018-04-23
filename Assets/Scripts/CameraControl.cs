using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    private GameObject followThis;

    void Start() {
		Camera.main.orthographicSize = 6;
        if ( followThis == null )
            return;
    }

    void LateUpdate() {
        if ( followThis == null )
            return;
        var target = followThis.transform.position;
        var targetVec = target - transform.position;
		targetVec.z = 0;
		Camera.main.orthographicSize = 6 + targetVec.sqrMagnitude * 10;
        transform.position = transform.position + targetVec;
        targetVec.Scale(new Vector3(0.05f, 0.05f, 0));
    }

    public void SetFollow(GameObject g) {
        followThis = g;
		var diff = (transform.position - followThis.transform.position);
		diff.Scale(new Vector3(1f, 1f, 0f));
		transform.position = transform.position - diff + (Vector3) (Random.insideUnitCircle) * Random.value * 5f;
    }
}