using UnityEngine;
using Unity.Cinemachine;

public class VehicleCameraController : MonoBehaviour
{
    public CinemachineCamera vCam;
    public Transform vehicle;
    private CinemachineRotationComposer composer;

    void Start()
    {
        composer = vCam.GetComponent<CinemachineRotationComposer>();
    }

    void LateUpdate()
    {
        Vector3 localVelocity = vehicle.InverseTransformDirection(vehicle.GetComponent<Rigidbody>().linearVelocity);
        if (localVelocity.z < 0) // vehicle moving backward
        {
            composer.TargetOffset.x = -Mathf.Abs(composer.TargetOffset.x);
        }
        else
        {
            composer.TargetOffset.x = Mathf.Abs(composer.TargetOffset.x);
        }
    }
}
