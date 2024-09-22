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
    [SerializeField] int spawnPerDifficultyIncrease;
    [SerializeField] int difficultyProgress = 0, maxBurst = 1;

    GameManager _game;

    void Start()
    {
        _game = GameManager.Instance;
        _game.OnGameStart.AddListener(() => StartCoroutine(SpawnRandomly()));
    }

    IEnumerator SpawnRandomly()
    {
        
        while (_game.IsGameStarted)
        {
            float delay = Random.Range(delayRange.x, delayRange.y);
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < Random.Range(1, maxBurst+1); i++)
            {
                float randomXOffset = Random.Range(-_offsetRange, _offsetRange);
                float randomZOffset = Random.Range(-_offsetRange, _offsetRange);

                Vector3 randomizedSpawnPosition = new(
                    transform.position.x + randomXOffset,
                    transform.position.y,
                    transform.position.z + randomZOffset
                );

                Vector3 forward = transform.forward;
                var randomizedDirection = RandomizeDirection(forward, _spreadAngle);

                Instantiate(_tearPrefab, randomizedSpawnPosition, randomizedDirection);
            }

            difficultyProgress++;
            if( difficultyProgress >= spawnPerDifficultyIncrease)
            {
                difficultyProgress = 0;
                maxBurst++;
            }
        }
    }

    Quaternion RandomizeDirection(Vector3 direction, float angleSpread)
    {
        float randomYaw = Random.Range(-angleSpread, angleSpread);
        float randomPitch = Random.Range(-angleSpread, angleSpread);

        Quaternion randomRotation = Quaternion.Euler(randomPitch, randomYaw, 0);

        return Quaternion.LookRotation(randomRotation * -direction);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red.WithAlpha(0.2f);
        Gizmos.DrawCube(transform.position, _offsetRange * new Vector3(2, 1 / _offsetRange, 2));

        // spread angle gizmos
        Gizmos.color = Color.red;
        var angle = _offsetRange + _spreadAngle / 2;
        var fwd = transform.forward * 3;
        Gizmos.DrawLine(transform.position + (transform.up * _offsetRange), transform.position + fwd + transform.up * angle);
        Gizmos.DrawLine(transform.position + (-transform.up * _offsetRange), transform.position + fwd + -transform.up * angle);
        Gizmos.DrawLine(transform.position + (-transform.right * _offsetRange), transform.position + fwd + -transform.right * angle);
        Gizmos.DrawLine(transform.position + (transform.right * _offsetRange), transform.position + fwd + transform.right * angle);

        var oldMtx = Gizmos.matrix;
        var mtx = Matrix4x4.TRS(transform.position + transform.forward * 3, Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), new(1, 0.01f, 1));
        Gizmos.matrix = mtx;
        Handles.matrix = mtx;
        Handles.Label(Vector3.zero, $"Spread angle: {_spreadAngle}");
        Gizmos.DrawWireSphere(Vector3.zero, _offsetRange + _spreadAngle / 2);
        Gizmos.matrix = oldMtx;
    }
#endif
}
