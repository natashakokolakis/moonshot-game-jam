using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGateController : MonoBehaviour
{
    private BoxCollider gateCollider;
    CliffFalling cliffFalling;
    //private MeshRenderer gateRenderer;

    private void Awake()
    {
        gateCollider = GetComponent<BoxCollider>();
        cliffFalling = GetComponentInChildren<CliffFalling>();
        //gateRenderer = GetComponent<MeshRenderer>();
    }

    private void OpenGate()
    {
        cliffFalling.StartFalling();
        //gateRenderer.enabled = false;
        //gateCollider.enabled = false;
        EventManagerNorth.StopListening("GolfBallSunk", OpenGate);
    }

    public void StopBacktracking()
    {
        if (gameObject)
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
