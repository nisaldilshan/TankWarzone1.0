using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotationspeed = Vector3.zero;
    private float tiltspeed = 0f;
    private float curretcameratilt = 0f;
    private Vector3 thrusterforce = Vector3.zero;

    [SerializeField]
    private float CameraRotationLimit = 85f;

    private Rigidbody rb;
    /////////////////////////////////////////
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    /////////////////////////////////////////
    //gets movement vector
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }
    /////////////////////////////////////////
    //gets rotational vector
    public void Rotate (Vector3 _Rotationspeed)
    {
        rotationspeed = _Rotationspeed;
    }
    /////////////////////////////////////////
    //gets tilting vector
    public void Tilt(float _Tiltspeed)
    {
        tiltspeed = _Tiltspeed;
    }
    /////////////////////////////////////////
    //get forces for the Thruster
    public void ApplyThruster(Vector3 _thrusterforce)
    {
        thrusterforce = _thrusterforce;
    }
    /////////////////////////////////////////
    //run every physics iteration
    void FixedUpdate ()
    {
        PerformMovement();
        PerformRotation();
    }
    /////////////////////////////////////////
    //perform movement based on velosity
    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterforce != Vector3.zero)
        {
            rb.AddForce(thrusterforce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    /////////////////////////////////////////
    //perform rotation
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationspeed)); 
        if(cam != null)
        {
            //set tilt and tiltlimits
            curretcameratilt -= tiltspeed;
            curretcameratilt = Mathf.Clamp(curretcameratilt, -CameraRotationLimit, CameraRotationLimit);
            //apply tilt
            cam.transform.localEulerAngles = new Vector3(curretcameratilt, 0f, 0f);
        }
    }
}
