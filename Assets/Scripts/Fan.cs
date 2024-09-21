using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [field: SerializeField]
    public bool IsOn { get; set; }

    [Header("References")]
    [SerializeField] Transform _fanHead;
    [SerializeField] Transform _fanBlades;
    [SerializeField] AudioSource _audio;
    [SerializeField] Transform _fanBlowPoint;

    [Header("Settings")]
    [SerializeField] float _rotateSpeed = 1;
    [SerializeField] float _fanBladeSpeed = 10;
    [SerializeField] float _fanForce = 5;
    [SerializeField] float _maxAngle = 45;
    [SerializeField] float _fanRange;

    float _startY;
    float _elapsed; // so it can maintain the rot when i turn it off

    void Start()
    {
        _startY = _fanHead.rotation.eulerAngles.y;
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
        _fanBlades.transform.Rotate(new Vector3(0, 0, _fanBladeSpeed));

        float y = Mathf.Sin(_elapsed * _rotateSpeed) * _maxAngle;
        _fanHead.rotation = Quaternion.Euler(0, _startY + y, 0);

        if (Physics.BoxCast(_fanBlowPoint.position, Vector3.one * 2, _fanBlowPoint.forward, out RaycastHit hit, _fanBlowPoint.rotation, _fanRange))
        {
            if (!hit.collider.TryGetComponent(out Rigidbody rb)) return;

            rb.AddForce(_fanBlowPoint.forward * _fanForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.2f);
        Gizmos.matrix = _fanHead.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(_fanBlowPoint.localPosition.x, _fanBlowPoint.localPosition.y, _fanBlowPoint.localPosition.z + _fanRange / 2), new Vector3(4f, 4f, _fanRange));
    }
}
