using UnityEngine;

public class PaperHealth : MonoBehaviour
{
    [SerializeField] private Mesh[] _meshStates;
    [SerializeField] private int _maxHP = 3;
    private int _currHP;
    private MeshFilter _filter;

    void Start()
    {
        _filter = GetComponent<MeshFilter>();
        _currHP = _maxHP;
        UpdateMesh();
    }

    public void TakeDamage(int damage)
    {
        _currHP -= damage;
        if (_currHP <= 0)
        {
            GameManager.Instance.GameOver();
        }
        UpdateMesh();
    }

    void UpdateMesh()
    {
        if (_currHP - 1 < 0) return;
        _filter.mesh = _meshStates[_currHP - 1];
    }
}
