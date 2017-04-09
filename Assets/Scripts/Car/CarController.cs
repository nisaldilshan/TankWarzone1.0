using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CarMotor))]
public class CarController : MonoBehaviour {

    public float maxTorque = 50f;
    public float maxSteerAngle = 45f;
    [SerializeField]
    private float gunsensitivity = 2f;

    //Sound
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch;
    private float steerwheel;
    private float accelerate;

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
        //Sound
        m_OriginalPitch = m_MovementAudio.pitch;

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
        steerwheel = Input.GetAxis("Horizontal");
        accelerate = Input.GetAxis("Vertical");
        // Debug.Log("Steerwheel = "+steerwheel + " Accelerate = "+accelerate);
        float _torque = maxTorque * accelerate;
        float _steer = steerwheel * maxSteerAngle;
        // Debug.Log("Torque = "+_torque + " Steer = "+_steer);                //Maximum steer 35 & -35

        //Sound
        EngineAudio();

        //apply movement
        motor.Move(_torque,_steer);

        //Animate movement
        animator.SetFloat("ForwardVelocity", accelerate);

        //calculate rotation as a 3d vector
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _rotationspeed = -_yRot * gunsensitivity;
        // Debug.Log("Rotation Speed = "+_rotationspeed);

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
                //Activate JUMP effects
                motor.Activate_JUMP_effects(true);
                //
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            //Deactivate JUMP effects
            motor.Activate_JUMP_effects(false);
            //
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        //apply thruster force
        motor.ApplyThruster(_thrusterforce);
    }










    private void EngineAudio(){
        //Check if tank is idle     
        if(Mathf.Abs(accelerate)<0.1f){
            //Checking playing clip and changing it to idling
            if(m_MovementAudio.clip == m_EngineDriving){
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else{
            //Checking playing clip and changing it to Driving
            if(m_MovementAudio.clip == m_EngineIdling){
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }

    }

}
