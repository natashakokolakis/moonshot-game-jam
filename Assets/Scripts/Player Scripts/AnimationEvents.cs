using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    PlayerMoonGolfController playerController;
    OrbGolfingScript golfingScript;
    FollowOrb followOrb;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerMoonGolfController>();
        golfingScript = GameObject.FindGameObjectWithTag("GolfBall").GetComponent<OrbGolfingScript>();
        followOrb = GetComponentInParent<FollowOrb>();
    }

    public void ShootAttackBall()
    {
        playerController.ShootAttackBall();
    }

    public void ShootGolfBall()
    {
        Debug.Log("shooting ball");
        golfingScript.ShootGolfBall(golfingScript.golfForce, golfingScript.attackDirection);
    }

    public void ReactivatePlayer()
    {
        Debug.Log("reactivating player");
        StartCoroutine(followOrb.MoveDummyToPlayerPosition(golfingScript.playerGO));
    }
}
