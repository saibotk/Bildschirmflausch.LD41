using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float power = 3;
    public float turnpower = 2;
    public float curveSpeed = 0.5f;
    Vector3 curspeed;
    Rigidbody2D rigidbody2D;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate() {
        
        curspeed = new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y, 0);

        //curspeed = curspeed.normalized;
        //curspeed *= maxspeed;

        rigidbody2D.AddForce(transform.up * power);
        Vector3 forwardNorm = (transform.localRotation * Vector3.up).normalized * curspeed.magnitude;
        Vector3 br = forwardNorm - curspeed;
        br *= curveSpeed;
        rigidbody2D.AddForce(br);
        //Debug.Log(br);

        if (Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward * turnpower);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.forward * -turnpower);
        }
        // Debug lines
        Debug.DrawLine(transform.position, transform.position + curspeed, Color.magenta, 0.01f, false);
        Debug.DrawLine(transform.position, transform.position + br, Color.red, 0.01f, false);
        Debug.DrawLine(transform.position, transform.position + transform.localRotation * Vector3.up, Color.yellow, 0.01f, false);

        //Debug.Log(transform.localRotation.eulerAngles);
        //Debug.Log(transform.localRotation * Vector3.up);
        //Debug.Log(curspeed);
    }
}
