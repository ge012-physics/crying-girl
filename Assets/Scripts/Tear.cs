using UnityEngine;

public class Tear : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] GameObject _tearHitParticlePrefab;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.forward * -Physics.gravity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        // TODO: game over or health decreasee when paper hit
    }
}
