using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private bool firstKeyPressed;

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

    Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if ( !firstKeyPressed && Input.anyKey ) {
            firstKeyPressed = true;
        }
    }

    void FixedUpdate() {
        if ( !firstKeyPressed )
            return;

        Vector3 speedVec = new Vector3(rb.velocity.x, rb.velocity.y, 0);
		Camera.main.orthographicSize = Camera.main.orthographicSize * 0.95f + (5 + 4f/6f * (speedVec.magnitude)) * 0.05f;
        float speed = speedVec.magnitude;

		{ // Forward
			Vector3 acc = transform.up * acceleration;
            if (Input.GetKey(KeyCode.S))
                acc *= 0;
            rb.AddForce(acc);
        }
		{// Drag
			Vector3 drag = speedVec.normalized * speed * speed * friction * -1;
			if (Input.GetKey(KeyCode.S)) {
				drag *= brake;
				drag *= speed;
     		}
            rb.AddForce(drag);
            Debug.DrawLine(transform.position, transform.position + drag, Color.cyan, 0.01f, false);
        }
        { // Drift
            Vector3 forwardNorm = ( transform.localRotation * Vector3.up ).normalized * speedVec.magnitude;
            Vector3 br = forwardNorm - speedVec;
            br *= drift;
            rb.AddForce(br);
            //Debug.Log(br);
            Debug.DrawLine(transform.position, transform.position + br, Color.red, 0.01f, false);
        }
        if ( Input.GetKey(KeyCode.A) )
            rb.MoveRotation(rb.rotation + turnSpeed);
        //transform.Rotate(Vector3.forward * turnSpeed);
        if ( Input.GetKey(KeyCode.D) )
            rb.MoveRotation(rb.rotation - turnSpeed);
        //transform.Rotate(Vector3.forward * -turnSpeed);
        // Debug lines
        Debug.DrawLine(transform.position, transform.position + speedVec, Color.magenta, 0.01f, false);
        Debug.DrawLine(transform.position, transform.position + transform.localRotation * Vector3.up, Color.yellow, 0.01f, false);

        //Debug.Log(transform.localRotation.eulerAngles);
        //Debug.Log(transform.localRotation * Vector3.up);
        //Debug.Log(curspeed);
    }
}
