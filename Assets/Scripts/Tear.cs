using DG.Tweening;
using UnityEngine;

public class Tear : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] GameObject _tearHitParticlePrefab;
    [SerializeField] GameObject _predictPrefab;
    [SerializeField] Renderer _renderer;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        var endColor = _renderer.material.color;

        var startColor = endColor;
        startColor.a = 0;

        _renderer.material.SetColor("_BaseColor", startColor);
        _renderer.material.DOColor(endColor, 2f);

        
        RaycastHit[] hits = Physics.RaycastAll(gameObject.transform.position, -gameObject.transform.forward, 100f);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Table"))
            {
                Vector3 spawnPosition = hit.point;

                Instantiate(_predictPrefab, spawnPosition, Quaternion.identity);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.forward * Physics.gravity.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PaperHealth paper))
        {
            paper.TakeDamage(1);
        }

        Destroy(gameObject);
        // TODO: game over or health decreasee when paper hit
    }

}
