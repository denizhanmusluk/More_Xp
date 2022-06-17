using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapToPlay : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}