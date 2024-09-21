using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearSpawn : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * -Physics.gravity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        // TODO: game over or health decreasee when paper hit
    }
}
