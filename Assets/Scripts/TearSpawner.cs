using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; 
    public Transform spawnPoint;
    public float spreadAngle = 5f;
    public float offsetRange = 5f;
    public Vector2 delayRange = new Vector2(1f, 5f);


    void Start()
    {
        StartCoroutine(SpawnRandomly());
    }
    IEnumerator SpawnRandomly()
    {
        while (true)
        {

            float delay = Random.Range(delayRange.x, delayRange.y);

            yield return new WaitForSeconds(delay);


            float randomXOffset = Random.Range(-offsetRange, offsetRange);
            float randomZOffset = Random.Range(-offsetRange, offsetRange);

            Vector3 randomizedSpawnPosition = new Vector3(
                spawnPoint.position.x + randomXOffset,
                spawnPoint.position.y,
                spawnPoint.position.z + randomZOffset
            );

            Vector3 forward = spawnPoint.transform.forward;
            var randomizedDirection = RandomizeDirection(forward, spreadAngle);

            var tear = Instantiate(objectToSpawn, randomizedSpawnPosition, randomizedDirection);
           
        }
    }

    Quaternion RandomizeDirection(Vector3 direction, float angleSpread)
    {
        float randomYaw = Random.Range(-angleSpread, angleSpread);
        float randomPitch = Random.Range(-angleSpread, angleSpread);

        Quaternion randomRotation = Quaternion.Euler(randomPitch, randomYaw, 0);

        return Quaternion.LookRotation(randomRotation * direction);
    }
}
