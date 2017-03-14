using UnityEngine;
using System.Collections;

public class CameraSetup : MonoBehaviour
{
    [SerializeField]
    Behaviour CameraBlur;

    void Update ()
    {
	    if(PauseMenu.IsOn)
            CameraBlur.enabled = true;
        else
            CameraBlur.enabled = false;
    }
}
