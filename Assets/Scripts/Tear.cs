using DG.Tweening;
using Unity.VisualScripting;
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
        var startColor = _renderer.material.color;
        _renderer.material.SetColor("_BaseColor", startColor.WithAlpha(0));
        _renderer.material.DOColor(startColor, 2f);

        
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
