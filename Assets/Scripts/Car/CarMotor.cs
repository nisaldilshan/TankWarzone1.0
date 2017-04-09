using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMotor : MonoBehaviour {

    [SerializeField]
    private Transform centerOfMass;
    [SerializeField]
    private Transform turret;
    [SerializeField]
    private Transform gun;

    private float rotationspeed = 0f;
    private float curret_turret_rotation = 0f;
    private float tiltspeed = 0f;
    private float curret_gun_tilt = 0f;

    [SerializeField]
    private float turretRotationLimit = 85f;
    [SerializeField]
    private float gunTiltLimit = 25f;

    public WheelCollider[] wheelColliders = new WheelCollider[4];

    private float torque = 0f;
    private float steer = 0f;
    private Vector3 thrusterforce = Vector3.zero;
    //thruster places
    private Vector3 thrusterplace1 = Vector3.zero;
    private Vector3 thrusterplace2 = Vector3.zero;
    private Vector3 thrusterplace3 = Vector3.zero;
    private Vector3 thrusterplace4 = Vector3.zero;

    //thruster effects
    [SerializeField]
    private GameObject thruster_eff1;
    [SerializeField]
    private GameObject thruster_eff2;
    [SerializeField]
    private GameObject thruster_eff3;
    [SerializeField]
    private GameObject thruster_eff4;

    [SerializeField]
    private GameObject thruster1;
    [SerializeField]
    private GameObject thruster2;
    [SerializeField]
    private GameObject thruster3;
    [SerializeField]
    private GameObject thruster4;

    //Sounds
    public AudioSource m_Boosters;
    public AudioClip m_Boost;

    private Rigidbody rb;
    /////////////////////////////////////////
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        
    }
    /////////////////////////////////////////
    //gets movement vector
    public void Move (float _torque, float _steer)
    {
        torque = _torque;
        steer = _steer;
    }
    /////////////////////////////////////////
    //gets rotational vector
    public void Rotate(float _rotationspeed)
    {
        rotationspeed = _rotationspeed;
    }
    /////////////////////////////////////////
    //gets rotational vector
    public void Tilt(float _tiltspeed)
    {
        tiltspeed = _tiltspeed;
    }
    /////////////////////////////////////////
    //get forces for the Thruster
    public void ApplyThruster(Vector3 _thrusterforce)
    {
        thrusterforce = _thrusterforce;        
    }

    /////////////////////////////////////////
    //get forces for the Thruster
    public void Activate_JUMP_effects(bool state)
    {
        thruster_eff1.SetActive(state);
        thruster_eff2.SetActive(state);
        thruster_eff3.SetActive(state);
        thruster_eff4.SetActive(state);
    }
    /////////////////////////////////////////

    void Update()
    {
        Vector3 p1 = transform.position;
        Vector3 p2 = transform.localEulerAngles;
        GameManagerCar.instance.getminimap_Values(p1[0], p1[2], p2[1]);
    }

    ///\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //run every physics iteration
    void FixedUpdate ()
    {
        thrusterplace1 = thruster1.transform.position;
        thrusterplace2 = thruster2.transform.position;
        thrusterplace3 = thruster3.transform.position;
        thrusterplace4 = thruster4.transform.position;

        PerformMovement();
        PerformRotation();
        PerformTilt();

        
    }
    /////////////////////////////////////////
    //perform movement based on input
    void PerformMovement()
    {
        wheelColliders[0].steerAngle = steer;
        wheelColliders[1].steerAngle = steer;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = torque;
        }

        if (thrusterforce != Vector3.zero)
        {
            //front
            rb.AddForceAtPosition(thrusterforce * Time.fixedDeltaTime, thrusterplace1, ForceMode.Impulse); //Acceleration
            rb.AddForceAtPosition(thrusterforce * Time.fixedDeltaTime, thrusterplace2, ForceMode.Impulse);
            //back
            rb.AddForceAtPosition(thrusterforce * Time.fixedDeltaTime, thrusterplace3, ForceMode.Impulse);
            rb.AddForceAtPosition(thrusterforce * Time.fixedDeltaTime, thrusterplace4, ForceMode.Impulse);
            Debug.Log("Wee!");
            //Sound
        	BoostAudio();
        }
    }

    /////////////////////////////////////////
    //perform turret rotation
    void PerformRotation()
    {
        //calculate rotation
        curret_turret_rotation -= rotationspeed;
        curret_turret_rotation = Mathf.Clamp(curret_turret_rotation, -turretRotationLimit, turretRotationLimit);
        //apply rotation
        turret.transform.localEulerAngles = new Vector3(0f, curret_turret_rotation, 0f);
        
    }
    /////////////////////////////////////////
    //perform gun tilt
    void PerformTilt()
    {
        //calculate tilt
        curret_gun_tilt -= tiltspeed;
        curret_gun_tilt = Mathf.Clamp(curret_gun_tilt, -gunTiltLimit, gunTiltLimit);
        //apply tilt
        gun.transform.localEulerAngles = new Vector3(curret_gun_tilt, 0f, 0f);

    }

    private void BoostAudio(){
        m_Boosters.clip = m_Boost;
        m_Boosters.Play();
    }


}
