﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class CanvasObjectBounce : MonoBehaviour
{
    private RectTransform rectTransform;
    private Tween tweenPos;
    public float amount = 5f;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tweenPos = rectTransform.DOAnchorPos3DY(rectTransform.anchoredPosition.y + amount, 1).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        tweenPos.Play();
    }

    private void OnDisable()
    {
        tweenPos.Pause();
    }

}
