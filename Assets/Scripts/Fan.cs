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
    [SerializeField] AudioClip _switchSound;
    [SerializeField] Renderer _powerIndicator;

    float _startY;
    float _elapsed; // so it can maintain the rot when i turn it off

    void Start()
    {
        _startY = _fanHead.rotation.eulerAngles.y;
        ToggleOn(state: false, force: true);
    }

    public void ToggleOn(bool state = true, bool force = false)
    {
        IsOn = force ? state : !IsOn;
        _audio.PlayOneShot(_switchSound);
        if (!force)
            GameManager.Instance.AddFanToConcurrency(this);

        if (IsOn)
            _audio.Play();
        else
            _audio.Stop();

        _powerIndicator.material.SetColor("_BaseColor", IsOn ? Color.green : Color.red);
    }

    void Update()
    {
        if (!IsOn) return;
        _elapsed += Time.deltaTime;
        _fanBlades.transform.Rotate(new Vector3(0, 0, _fanBladeSpeed));

        float y = Mathf.Sin(_elapsed * _rotateSpeed) * _maxAngle;
        _fanHead.rotation = Quaternion.Euler(0, _startY + y, 0);

        var cols = Physics.OverlapBox(_fanBlowPoint.position, (Vector3.one * 2) + Vector3.forward * _fanRange, _fanBlowPoint.rotation);

        foreach (var col in cols)
        {
            if (col.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(_fanBlowPoint.forward * _fanForce);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.2f);
        Gizmos.matrix = _fanHead.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(_fanBlowPoint.localPosition.x, _fanBlowPoint.localPosition.y, _fanBlowPoint.localPosition.z + _fanRange / 2), new Vector3(4f, 4f, _fanRange));
    }
}
