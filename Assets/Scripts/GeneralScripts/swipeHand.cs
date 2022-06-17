using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class swipeHand : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveX(Screen.width/3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
