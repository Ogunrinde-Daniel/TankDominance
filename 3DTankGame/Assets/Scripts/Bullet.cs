using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Tank owner;
    public ParticleSystem hit;
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
            return;
        if ( (collision.GetComponent<Tank>() != null) && collision.GetComponent<Tank>() == owner){return;}

        Instantiate(hit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
