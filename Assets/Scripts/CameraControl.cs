using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	public GameObject followThis;

	private Vector3 offset;

	void Start()
	{
		offset = transform.position - followThis.transform.position;
	}

	void LateUpdate()
	{
		var target = followThis.transform.position + offset;
		var targetVec = target - transform.position;
		targetVec.Scale (new Vector3 (0.05f, 0.05f, 0));

		transform.position = transform.position + targetVec;
	}
}