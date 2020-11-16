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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GolfBall"))
            {
                EventManagerNorth.TriggerEvent("GolfBallSunk");
                Debug.Log("Hole Complete!");
            }
    }
}
