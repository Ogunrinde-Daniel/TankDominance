using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TankTurret : MonoBehaviour
{
    public Transform target;            // The target to rotate towards
    public float rotationSpeed = 5f;    // The speed at which to rotate the turret
    public bool targetReached = false;
    private void LateUpdate()
    {
        if (target == null) return;

        // Get the direction to the target
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Keep the turret level

        if (direction != Vector3.zero)
        {
            // Rotate the turret smoothly towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            targetReached = HasTransformReachedRotation(transform, targetRotation);
        }
    }

    public bool HasTransformReachedRotation(Transform transform, Quaternion targetRotation, float tolerance = 0.5f)
    {
        return Quaternion.Angle(transform.rotation, targetRotation) < tolerance;
    }
}
