using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tear : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] GameObject _tearHitParticlePrefab;
    [SerializeField] Renderer _renderer;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        var startColor = _renderer.material.color;
        _renderer.material.SetColor("_BaseColor", startColor.WithAlpha(0));
        _renderer.material.DOColor(startColor, 2f);
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.forward * -Physics.gravity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PaperHealth paper))
        {
            paper.TakeDamage(1);
        }

        Destroy(gameObject);
        // TODO: game over or health decreasee when paper hit
    }
}
