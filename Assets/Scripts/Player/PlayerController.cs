using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float looksensitivity = 3f;

    [SerializeField]
    private float thrusterforce = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.2f;

    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Spring Settings:")]
    //[SerializeField]
    //private JointDriveMode jointmode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    //component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;
    /////////////////////////////////////////
    void Start ()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }
    /////////////////////////////////////////
    void Update()
    {
        if (PauseMenu.IsOn)
            return;

        //calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        //Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        //apply movement
        motor.Move(_velocity);

        //calculate rotation as a 3d vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _Rotationspeed = new Vector3(0f, _yRot, 0f) * looksensitivity;

        //apply rotation
        motor.Rotate(_Rotationspeed);

        //calculate tilt as a 3d vector
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _tiltspeed = _xRot * looksensitivity;

        //apply tilt
        motor.Tilt(_tiltspeed);

        //calculate thruster force
        Vector3 _thrusterforce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if (thrusterFuelAmount > 0.01f)
            {
                _thrusterforce = Vector3.up * thrusterforce;
                SetJointSettings(0f);
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        //apply thruster force
        motor.ApplyThruster(_thrusterforce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            //mode = jointmode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
