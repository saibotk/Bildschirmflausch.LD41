using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    bool firstKeyPressed;
    bool messagePosted;

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
	[SerializeField]
	public float maxBrakeTime = 5f;

    // The time of the acceleration/deceleration sounds in seconds
	[SerializeField]
	public float accelerationTime = 5;
	[SerializeField]
	public float decelerationTime = 5;

	float brakeTime;
	float lastFrame;

	public enum SpeedState
	{
		SLOW, FASTER, FAST, SLOWER
	}

	SpeedState state;
	double changeTime;

    Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        messagePosted = false;
		state = SpeedState.SLOW;
		brakeTime = 0;
		lastFrame = Time.time;
        GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.slowdriving);
    }

    void Update() {
        if (!firstKeyPressed && !messagePosted) {
            messagePosted = true;
            GameController.instance.GetUI().GetNotificationManager().ShowMessage("Press any key to start!", 2);
        }
        if ( !firstKeyPressed && Input.anyKey ) {
            firstKeyPressed = true;
			if (Input.GetAxis("Vertical") >= 0) {            
				state = SpeedState.FASTER;
				changeTime = Time.time;
				GameController.instance.GetAudioControl().SfxStop(AudioControl.Sfx.slowdriving);
				GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.faster);
            }
        }
    }

    void FixedUpdate() {
        if ( !firstKeyPressed )
            return;

        Vector3 speedVec = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        float speed = speedVec.magnitude;
  
		bool braking = Input.GetAxis("Vertical") < 0;
		if (braking && brakeTime >= maxBrakeTime) {
			brakeTime = maxBrakeTime;
			braking = false;
		} else if (!braking) {
			brakeTime -= (Time.time - lastFrame) * 0.1f;
		}
		Debug.Log(braking + " " + brakeTime);
		if (braking) {
			brakeTime += Time.time - lastFrame;
            GameController.instance.GetAudioControl().SfxStop(AudioControl.Sfx.driving);
			switch (state) {
				case SpeedState.FASTER:
					if (Time.time - changeTime > accelerationTime)
					{
						changeTime = Time.time;
                        state = SpeedState.SLOWER;
						GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.slower);
					}
					break;
				case SpeedState.FAST:
					changeTime = Time.time;
					state = SpeedState.SLOWER;
                    GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.slower);
					break;
				case SpeedState.SLOWER:
					if (Time.time - changeTime > decelerationTime)
					{
						state = SpeedState.SLOW;
						GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.slowdriving);
					}
					break;
				case SpeedState.SLOW:
					break;
			}
		} else {
			if (brakeTime < 0)
				brakeTime = 0;
			GameController.instance.GetAudioControl().SfxStop(AudioControl.Sfx.slowdriving);
            switch (state)
            {
                case SpeedState.FASTER:
					if (Time.time - changeTime > accelerationTime) {
						state = SpeedState.FAST;
						GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.driving);
                    }
                    break;
                case SpeedState.FAST:
                    break;
                case SpeedState.SLOWER:
					if (Time.time - changeTime > decelerationTime) {
						changeTime = Time.time;
						state = SpeedState.FASTER;
						GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.faster);
                    }
                    break;
				case SpeedState.SLOW:
                    changeTime = Time.time;
					state = SpeedState.FASTER;
					GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.faster);
					break;
            }
		}

		{ // Forward
			Vector3 acc = transform.up * acceleration;
            if (braking)
                acc *= 0;
            rb.AddForce(acc);
        }
		{// Drag
			Vector3 drag = speedVec.normalized * speed * speed * friction * -1;
			if (braking) {
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
        //transform.Rotate(Vector3.forward * turnSpeed);
        if (Input.GetAxis("Horizontal") < 0)
        {
			//rb.rotation += turnSpeed;
            rb.MoveRotation(rb.rotation + turnSpeed);
        } else
        //transform.Rotate(Vector3.forward * turnSpeed);
        if (Input.GetAxis("Horizontal") > 0)
        {
			//rb.rotation -= turnSpeed;
			rb.MoveRotation(rb.rotation - turnSpeed);
		} else {
			rb.MoveRotation(rb.rotation);
		}
        //transform.Rotate(Vector3.forward * -turnSpeed);
        // Debug lines
        Debug.DrawLine(transform.position, transform.position + speedVec, Color.magenta, 0.01f, false);
        Debug.DrawLine(transform.position, transform.position + transform.localRotation * Vector3.up, Color.yellow, 0.01f, false);

		//Debug.Log(transform.localRotation.eulerAngles);
		//Debug.Log(transform.localRotation * Vector3.up);
		//Debug.Log(curspeed);
		lastFrame = Time.time;
    }

    /// <returns>The time in seconds the player was braking</returns>
	public float GetBrakeTime() {
		return brakeTime;
	}
}
