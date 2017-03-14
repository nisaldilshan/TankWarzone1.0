using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CarMotor))]
public class CarController : MonoBehaviour {

    public float maxTorque = 50f;
    public float maxSteerAngle = 45f;
    [SerializeField]
    private float gunsensitivity = 2f;

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

    //component caching
    private CarMotor motor;
    private Animator animator;
    /////////////////////////////////////////
    void Start ()
    {
        motor = GetComponent<CarMotor>();
        animator = GetComponent<Animator>();
    }
    /////////////////////////////////////////
    void Update()
    {
        if (PauseMenu.IsOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            motor.Move(0f, 0f);
            motor.Rotate(0f);
            motor.Tilt(0f);

            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //calculate movement velocity as a 3D vector
        float steerwheel = Input.GetAxis("Horizontal");
        float accelerate = Input.GetAxis("Vertical");
        float _torque = maxTorque * accelerate;
        float _steer = steerwheel * maxSteerAngle;
        //apply movement
        motor.Move(_torque,_steer);

        //Animate movement
        animator.SetFloat("ForwardVelocity", accelerate);

        //calculate rotation as a 3d vector
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _rotationspeed = -_yRot * gunsensitivity;
        //apply rotation
        motor.Rotate(_rotationspeed);

        //calculate tilt as a 3d vector
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _tiltspeed = _xRot * gunsensitivity;
        //apply tilt
        motor.Tilt(_tiltspeed);
        /////////////////////////////////////////////////////

        //calculate thruster force
        Vector3 _thrusterforce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if (thrusterFuelAmount > 0.01f)
            {
                _thrusterforce = Vector3.up * thrusterforce;
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        //apply thruster force
        motor.ApplyThruster(_thrusterforce);
    }

}
