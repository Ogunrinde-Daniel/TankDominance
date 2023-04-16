using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public float spawnRange = 5f;     // the range of positions along the x-axis to spawn
    public float spawnHeight = 1.35f;  // the height at which to spawn the object

    void Update()
    {

    }

    public Vector3 getRandomPos()
    {
        float spawnPositionX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(spawnPositionX, spawnHeight, 0f);
        return spawnPosition;
    }

}
