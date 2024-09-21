using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TearSpawner : MonoBehaviour
{
    [SerializeField] GameObject _tearPrefab;
    [SerializeField] float _spreadAngle = 5f;
    [SerializeField] float _offsetRange = 5f;
    [SerializeField] Vector2 delayRange = new(1f, 5f);


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


            float randomXOffset = Random.Range(-_offsetRange, _offsetRange);
            float randomZOffset = Random.Range(-_offsetRange, _offsetRange);

            Vector3 randomizedSpawnPosition = new(
                transform.position.x + randomXOffset,
                transform.position.y,
                transform.position.z + randomZOffset
            );

            Vector3 forward = transform.transform.forward;
            var randomizedDirection = RandomizeDirection(forward, _spreadAngle);

            Instantiate(_tearPrefab, randomizedSpawnPosition, randomizedDirection);
        }
    }

    Quaternion RandomizeDirection(Vector3 direction, float angleSpread)
    {
        float randomYaw = Random.Range(-angleSpread, angleSpread);
        float randomPitch = Random.Range(-angleSpread, angleSpread);

        Quaternion randomRotation = Quaternion.Euler(randomPitch, randomYaw, 0);

        return Quaternion.LookRotation(randomRotation * direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red.WithAlpha(0.2f);
        Gizmos.DrawCube(transform.position, _offsetRange * new Vector3(2, 1 / _offsetRange, 2));
    }
}
