using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGateController : MonoBehaviour
{
    private MeshCollider gateCollider;
    private MeshRenderer gateRenderer;

    private void Awake()
    {
        gateCollider = GetComponent<MeshCollider>();
        gateRenderer = GetComponent<MeshRenderer>();
    }

    private void OpenGate()
    {
        gateRenderer.enabled = false;
        gateCollider.enabled = false;
        EventManagerNorth.StopListening("GolfBallSunk", OpenGate);
    }

    public void StopBacktracking()
    {
        gateCollider.enabled = true;
    }

    private void OnEnable()
    {
        EventManagerNorth.StartListening("GolfBallSunk", OpenGate);
    }

    private void OnDisable()
    {
        EventManagerNorth.StopListening("GolfBallSunk", OpenGate);
    }

}
