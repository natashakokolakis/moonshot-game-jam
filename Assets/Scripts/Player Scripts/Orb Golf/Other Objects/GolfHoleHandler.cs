using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfHoleHandler : MonoBehaviour
{
    private BoxCollider golfHoleCollider;

    private void Awake()
    {
        golfHoleCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("GolfBall") & (!other.isTrigger))
            {
                EventManagerNorth.TriggerEvent("GolfBallSunk");
                Debug.Log("Hole Complete!");
            }
    }
}
