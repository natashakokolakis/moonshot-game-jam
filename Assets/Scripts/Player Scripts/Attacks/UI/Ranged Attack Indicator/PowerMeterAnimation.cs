using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PowerMeterAnimation : MonoBehaviour
{
    public Tween powerBarLength;
    public Tween powerBarColour;
    public float tweenCurrentTime = 0f;
    
    public PlayerMoonGolfController playerController;
    private LineRenderer lineRenderer;


    private void OnDisable()
    {
        powerBarColour.Pause();
        powerBarLength.Pause();
    }

    void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        DOTween.Init(true, true, LogBehaviour.Default);
        powerBarLength = transform.DOScale(Vector3.zero, 1).From().SetAutoKill(false).SetLoops(-1, LoopType.Restart);
        powerBarColour = lineRenderer.DOColor(new Color2(Color.green, Color.green), new Color2(Color.green, Color.red), 1).SetAutoKill(false).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        tweenCurrentTime = Mathf.Clamp((playerController.golfPower/ playerController.golfPowerMAX), 0f, 0.99f);
        powerBarLength.fullPosition = tweenCurrentTime;
        powerBarColour.fullPosition = tweenCurrentTime;
    }
}
