using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GolfHoleHandler : MonoBehaviour
{
    private BoxCollider golfHoleCollider;
    PlayerAnimations animatePlayer;

    [FMODUnity.EventRef]
    public string BallSinkEvent = "";

    public Material golfSunkMat;

    private MMFeedbacks sunkHoleFeedback;

    private void Awake()
    {
        sunkHoleFeedback = transform.GetComponentInChildren<MMFeedbacks>();
        golfHoleCollider = GetComponent<BoxCollider>();
        animatePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimations>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("GolfBall") & (!other.isTrigger))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/GateCrumble", transform.position);
            FMODUnity.RuntimeManager.PlayOneShot(BallSinkEvent, transform.position);
            sunkHoleFeedback.PlayFeedbacks();
            EventManagerNorth.TriggerEvent("GolfBallSunk");
            animatePlayer.GolfInHole();
            enabled = false;
            Debug.Log("Hole Complete!");
            golfHoleCollider.enabled = false;
            //other.gameObject.SetActive(false);
            other.gameObject.GetComponent<MeshRenderer>().material = golfSunkMat;
        }
    }
}
