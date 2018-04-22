using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    private GameObject followThis;

    private Vector3 offset;

    void Start() {
        if ( followThis == null )
            return;
        offset = transform.position - followThis.transform.position;
    }

    void LateUpdate() {
        if ( followThis == null )
            return;
        var target = followThis.transform.position; // + offset;
        var targetVec = target - transform.position;
        targetVec.Scale(new Vector3(0.05f, 0.05f, 0));

        transform.position = transform.position + targetVec;
    }

    public void SetFollow(GameObject g) {
        followThis = g;
        offset = transform.position - followThis.transform.position;
    }
}