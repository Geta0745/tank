using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerTurret : MonoBehaviour
{
    public Transform turret; // The transform of the turret
    public Transform gunMount; // The transform of the gun mount
    public float turretRotateSpeed = 10f; // The speed at which the turret rotates
    public float gunRotateSpeed = 10f; // The speed at which the gun mount rotates
    public Transform target; // The transform of the target
    public float angleThreshold = 5f; // The angle threshold for rotating the gun mount
    public float minRotation = -16f; // The minimum rotation of the gun mount
    public float maxRotation = 6f; // The maximum rotation of the gun mount

    // Update is called once per frame
    void Update()
    {
        // Rotate the turret towards the target
        Vector3 targetDirection = target.position - turret.position;
        targetDirection.y = 0f; // Ignore the y-axis rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        turret.localRotation = Quaternion.Slerp(turret.localRotation, targetRotation, Time.deltaTime * turretRotateSpeed);

        // Calculate the angle between the turret and the target
        float angle = Vector3.Angle(turret.forward, target.position - turret.position);

        // If the angle is greater than the threshold, rotate the gun mount towards the target
        if (angle > angleThreshold)
        {
            Quaternion gunTargetRotation = Quaternion.LookRotation(target.position - gunMount.position);

            // Only rotate the gun mount on the y-axis
            gunTargetRotation.y = 0f;
            gunTargetRotation.z = 0f;
            

            // Clamp the gun mount rotation to the min and max values
            gunTargetRotation = ClampRotation(gunTargetRotation, minRotation, maxRotation);
            gunMount.localRotation = Quaternion.Slerp(gunMount.localRotation, gunTargetRotation, Time.deltaTime * gunRotateSpeed);
        }
    }

    // Clamp the rotation of the gun mount to the min and max values
    Quaternion ClampRotation(Quaternion rot, float min, float max)
    {
        rot.x = Mathf.Clamp(rot.x, min, max);
        rot.y = Mathf.Clamp(rot.y, min, max);
        rot.z = Mathf.Clamp(rot.z, min, max);

        return rot;
    }
}
