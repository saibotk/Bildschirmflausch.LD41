using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	public float acceleration = 3;
	[SerializeField]
	public float friction = 0.1f;
	[SerializeField]
	public float turnSpeed = 2;
	[SerializeField]
	public float drift = 1f;
	[SerializeField]
	public float brake = 2f;

    Rigidbody2D rigidbody2D;

    // Use this for initialization
    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
		Vector3 speedVec = new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y, 0);
		float speed = speedVec.magnitude;

		{ // Forward
			rigidbody2D.AddForce(transform.up * acceleration);
        }
        {// Drag
			Vector3 drag = speedVec.normalized * speed * speed * friction * -1;
			if (Input.GetKey(KeyCode.S))
				drag *= brake;
            rigidbody2D.AddForce(drag);
            Debug.DrawLine(transform.position, transform.position + drag, Color.cyan, 0.01f, false);
        }
        { // Drift
			Vector3 forwardNorm = (transform.localRotation * Vector3.up).normalized * speedVec.magnitude;
			Vector3 br = forwardNorm - speedVec;
			br *= drift;
			rigidbody2D.AddForce(br);
			//Debug.Log(br);
			Debug.DrawLine(transform.position, transform.position + br, Color.red, 0.01f, false);
        }
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * turnSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.forward * -turnSpeed);
        // Debug lines
        Debug.DrawLine(transform.position, transform.position + speedVec, Color.magenta, 0.01f, false);
        Debug.DrawLine(transform.position, transform.position + transform.localRotation * Vector3.up, Color.yellow, 0.01f, false);

        //Debug.Log(transform.localRotation.eulerAngles);
        //Debug.Log(transform.localRotation * Vector3.up);
        //Debug.Log(curspeed);
    }
}
