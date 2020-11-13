using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OrbPowerIndicator : MonoBehaviour
{
    public Tween powerBarIndicatorPosition;
    public float tweenCurrentTime = 0f;
    public OrbGolfingScript orbController;

    private void OnDisable()
    {
        powerBarIndicatorPosition.Pause();
    }

    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        powerBarIndicatorPosition = transform.DOLocalMove(new Vector3(0, 0, 9), 1).SetAutoKill(false).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {

        tweenCurrentTime = Mathf.Clamp((orbController.golfPower / orbController.golfPowerMAX), 0f, 0.99f);
        powerBarIndicatorPosition.fullPosition = tweenCurrentTime;
    }
}
