using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfHoleHandler : MonoBehaviour
{
    private BoxCollider golfHoleCollider;
    PlayerAnimations animatePlayer;

    private void Awake()
    {
        golfHoleCollider = GetComponent<BoxCollider>();
        animatePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimations>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("GolfBall") & (!other.isTrigger))
        {
            EventManagerNorth.TriggerEvent("GolfBallSunk");
            animatePlayer.GolfInHole();
            Debug.Log("Hole Complete!");
        }
    }
}
