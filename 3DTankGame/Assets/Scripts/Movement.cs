using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Movement : MonoBehaviour
{
    [SerializeField]private Transform forward;
    [SerializeField]private Transform backward;

    [SerializeField]private float speed;
    [SerializeField]public float turnSpeed;

    [HideInInspector]public float dirX;
    [HideInInspector]public float dirZ;

    public WheelRotation lFront;
    public WheelRotation rFront;
    public WheelRotation lback;
    public WheelRotation rback;


    private Rigidbody rb;
    public bool isPlayer = false;

    private void Start()
    {
        if(isPlayer) rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isPlayer) return;
        if(dirZ == -1)//apply z movement onbly when moving
            transform.Rotate(0, dirX * -1 * turnSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0, dirX * turnSpeed * Time.deltaTime, 0);

        if (dirZ == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, forward.position, speed * Time.deltaTime);
        }
        else if (dirZ == -1)
        {
            transform.position = Vector3.MoveTowards(transform.position, backward.position, speed * Time.deltaTime);
        }

        UpdateWheels(dirZ == 0);
    }

    public void UpdateWheels(bool stopMoving){
        rFront.pauseRotate = stopMoving;
        lFront.pauseRotate = stopMoving;        
        rback.pauseRotate = stopMoving;
        lback.pauseRotate = stopMoving;
        
    }

    /*
        rotates smoothly towards a position
        and returns true if the rotation is completed
        has to be called at every frame
     */
    public bool SmoothRoate(Vector3 target)
    {
        //get the normalized direction
        Vector3 direction = transform.position - target;
        //calulate the angle difference--and convert it to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //calulate the final rotation
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //smothly rotate to the look dir over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        return true;

    }

    /*
        rotates smoothly towards a position
        and returns true if the rotation is completed
        has to be called at every frame
     */
    public bool SmoothRoate(Transform transform, Transform target)
    {
        //get the normalized direction
        Vector3 direction = target.position - transform.position;
        //calulate the angle difference--and convert it to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //calulate the final rotation
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //smothly rotate to the look dir over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        return CompareRotations(transform, target, 0.5f);

    }

    /*
     this checks if a's angle is looking at b's position
     and gets a tolerance to check
    */
    private bool CompareRotations(Transform a, Transform b,float tolerance)
    {
        //calculate the final look dir
        Quaternion lookDir = Quaternion.LookRotation(Vector3.forward, b.position - a.position);
        //calculate the angle dir -- the -90 fixes a bug only God understands:)
        float angleDifference = Quaternion.Angle(a.rotation, lookDir) - 90;
        return angleDifference <= tolerance;
    }


}
