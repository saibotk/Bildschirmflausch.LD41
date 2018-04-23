using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    private float followSpeed = 0.05f;
    [SerializeField]
    private float minZoom = 5;
    [SerializeField]
    private float zoomFactor = 4;
    [SerializeField]
    private float zoomSpeed = 0.05f;
	[SerializeField]
	private int startingDist = 5;
	[SerializeField]
	private GameObject followThis;

    void Start() {
		Camera.main.orthographicSize = minZoom;
        if ( followThis == null )
            return;
    }

    void LateUpdate() {
        if ( followThis == null )
            return;
        var target = followThis.transform.position;
        var targetVec = target - transform.position;
		targetVec.Scale(new Vector3(followSpeed, followSpeed, 0));
        transform.position = transform.position + targetVec;

		Camera.main.orthographicSize = Camera.main.orthographicSize * (1-zoomSpeed) + (minZoom + zoomFactor / 6f * (followThis.GetComponent<Rigidbody2D>().velocity.magnitude)) * zoomSpeed;
    }

    public void SetFollow(GameObject g) {
        followThis = g;
		var diff = (transform.position - followThis.transform.position);
		diff.Scale(new Vector3(1f, 1f, 0f));
		transform.position = transform.position - diff + (Vector3) (Random.insideUnitCircle) * Random.value * startingDist;
    }
}