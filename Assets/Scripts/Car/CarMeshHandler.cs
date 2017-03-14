using UnityEngine;

public class CarMeshHandler : MonoBehaviour {

    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];

    void Update()
    {
        UpdateMeshesPositions();
    }

    void UpdateMeshesPositions()
    {
        for(int i = 0; i < 4 ; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);
            tireMeshes[i].position = pos;
            tireMeshes[i].rotation = quat;
        }
    }
}
