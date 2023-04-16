using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//test class-can be deleted
public class ObjectProximity : MonoBehaviour
{ 
    public Transform playerTransform;
    public Transform[] objectTransforms;

    private void Start()
    {
        foreach (Transform objectTransform in objectTransforms)
        {
            float distance = Vector2.Distance(playerTransform.position, objectTransform.position);
            float proximityScale = 1f / (distance / 4 );
            proximityScale = Mathf.Lerp(1f, 10f, proximityScale);
            Debug.Log(objectTransform.name + " distance: " + distance + " proximity scale: " + proximityScale);

        }
    }

}
