using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Predict : MonoBehaviour
{
    [SerializeField] float duration;
    void Start()
    {
        transform.DOScaleX(1.5f, duration);
        transform.DOScaleZ(1.5f, duration);
    }

    private void Update()
    {
        if(transform.localScale.x >=1.5f) Destroy(gameObject);
    }
}
