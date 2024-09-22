using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraSignal : MonoBehaviour
{
    public static CameraSignal Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera()
    {
        transform.DOShakePosition(1, 1, 10, 90, false, true);
    }
}
