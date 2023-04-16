using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleeffectTest : MonoBehaviour
{
    public ParticleSystem hitEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate the hit effect at the position where the mouse clicked
                if (hitEffect != null)
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
            }
        }
    }
}
