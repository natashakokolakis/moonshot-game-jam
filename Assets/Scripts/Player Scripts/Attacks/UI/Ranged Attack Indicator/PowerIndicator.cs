using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerIndicator : MonoBehaviour
{
    public Tween powerBarIndicatorPosition;
    public float tweenCurrentTime = 0f;
    public PlayerMoonGolfController playerController;

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

        tweenCurrentTime = Mathf.Clamp((playerController.golfPower / playerController.golfPowerMAX), 0f, 0.99f);
        powerBarIndicatorPosition.fullPosition = tweenCurrentTime;
    }
}
