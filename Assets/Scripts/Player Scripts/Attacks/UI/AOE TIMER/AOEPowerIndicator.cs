using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AOEPowerIndicator : MonoBehaviour
{
    public Tween aoeCirclePosition;
    public float tweenCurrentTime = 0f;
    public AOEAttackHandler aoeHandler;

    private void OnDisable()
    {
        aoeCirclePosition.Pause();
    }

    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        aoeCirclePosition = transform.DOLocalMove(new Vector3(0, 0, 9), 1).SetAutoKill(false).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {

        tweenCurrentTime = Mathf.Clamp((aoeHandler.currentTime / aoeHandler.maxSpecialTimer), 0f, 0.99f);
        aoeCirclePosition.fullPosition = tweenCurrentTime;
    }
}
