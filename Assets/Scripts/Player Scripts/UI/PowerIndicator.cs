using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerIndicator : MonoBehaviour
{
    public Tween powerBarIndicatorPosition;
    public float tweenCurrentTime = 0f;
    public PlayerMoonGolfController playerController;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        powerBarIndicatorPosition = transform.DOLocalMove(new Vector3(0, 0, 9), 1).SetAutoKill(false).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        tweenCurrentTime = playerController.golfPower / playerController.golfPowerMAX;
        powerBarIndicatorPosition.fullPosition = tweenCurrentTime;
    }
}
