using UnityEngine;

public class CarCam : MonoBehaviour {

    [SerializeField]
    private float looksensitivity = 4f;
    [SerializeField]
    private Camera cam;

    private float cam_rot = 0f;
    private float curret__cam_rot = 0f;
    private float cam_til = 0f;
    private float curret__cam_til = 0f;

    [SerializeField]
    private float camRotLimit = 25f;
    [SerializeField]
    private float camTiltLimit = 25f;

    void Update () {
        ///////////////////////////////////////////////////
        //calculate cameratilt as a 3d vector
        float _cam_xRot = Input.GetAxisRaw("Mouse Y");
        cam_til = _cam_xRot * looksensitivity;
        //calculate camerarotation as a 3d vector
        float _cam_yRot = Input.GetAxisRaw("Mouse X");
        cam_rot = -_cam_yRot * looksensitivity;
        /////////////////////////////////////////////////////
    }

    void FixedUpdate()
    {
        Perform_Cam_Rot();
    }

    //perform camera rotation and tilt
    void Perform_Cam_Rot()
    {
        //calculate tilt
        curret__cam_til -= cam_til;
        curret__cam_til = Mathf.Clamp(curret__cam_til, -camTiltLimit, camTiltLimit);
        //calculate rotation
        curret__cam_rot -= cam_rot;
        curret__cam_rot = Mathf.Clamp(curret__cam_rot, -camRotLimit, camRotLimit);
        //apply rotation and tilt of camera
        cam.transform.localEulerAngles = new Vector3(curret__cam_til, curret__cam_rot, 0f);

    }
}
