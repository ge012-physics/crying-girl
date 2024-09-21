using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red.WithAlpha(0.2f);
        Gizmos.DrawCube(transform.position, _offsetRange * new Vector3(2, 1 / _offsetRange, 2));

        // spread angle gizmos
        Gizmos.color = Color.red;
        var angle = _offsetRange + _spreadAngle / 2;
        Gizmos.DrawLine(transform.position + (transform.forward * _offsetRange), transform.position + (-transform.up * 3) + transform.forward * angle);
        Gizmos.DrawLine(transform.position + (-transform.forward * _offsetRange), transform.position + (-transform.up * 3) + -transform.forward * angle);
        Gizmos.DrawLine(transform.position + (-transform.right * _offsetRange), transform.position + (-transform.up * 3) + -transform.right * angle);
        Gizmos.DrawLine(transform.position + (transform.right * _offsetRange), transform.position + (-transform.up * 3) + transform.right * angle);
        var oldMtx = Gizmos.matrix;
        var mtx = Matrix4x4.TRS(transform.position + (-transform.up * 3), transform.rotation, new(1, 0.01f, 1));
        Gizmos.matrix = mtx;
        Handles.matrix = mtx;
        Handles.Label(Vector3.zero, $"Spread angle: {_spreadAngle}");
        Gizmos.DrawWireSphere(Vector3.zero, _offsetRange + _spreadAngle / 2);
        Gizmos.matrix = oldMtx;
    }
#endif
}
