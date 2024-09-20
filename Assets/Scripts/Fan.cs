using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [field: SerializeField]
    public bool IsOn { get; set; }

    [SerializeField] float _rotateSpeed = 1;
    [SerializeField] float _fanForce = 5;
    [SerializeField] float _maxAngle = 45;
    [SerializeField] AudioSource _audio;

    [SerializeField] Transform _fanBlowPoint;
    [SerializeField] float _fanRange;

    float _startY;
    float _elapsed; // so it can maintain the rot when i turn it off

    void Start()
    {
        _startY = transform.rotation.eulerAngles.y;
    }

    public void ToggleOn()
    {
        // (zsfer): uncomment once we have audio
        // _audio.Play();
        IsOn = !IsOn;
    }

    void Update()
    {
        if (!IsOn) return;
        _elapsed += Time.deltaTime;

        float y = Mathf.Sin(_elapsed * _rotateSpeed) * _maxAngle;
        transform.rotation = Quaternion.Euler(0, _startY + y, 0);

        if (Physics.BoxCast(_fanBlowPoint.position, Vector3.one, transform.forward, out RaycastHit hit, transform.rotation, _fanRange))
        {
            if (!hit.collider.TryGetComponent(out Rigidbody rb)) return;

            rb.AddForce(transform.forward * _fanForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.2f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(_fanBlowPoint.localPosition.x, _fanBlowPoint.localPosition.y, _fanBlowPoint.localPosition.z + _fanRange / 2), new Vector3(2f, 2f, _fanRange));
    }
}
