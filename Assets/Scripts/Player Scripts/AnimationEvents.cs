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
        if (!transform.parent.CompareTag("Player"))
        golfingScript = transform.parent.transform.parent.GetComponentInChildren<OrbGolfingScript>();
        followOrb = GetComponentInParent<FollowOrb>();
    }

    public void ShootAttackBall()
    {
        playerController.ShootAttackBall();
    }

    public void ShootGolfBall()
    {

        golfingScript.ShootGolfBall(golfingScript.golfForce, golfingScript.attackDirectionLock);
    }

    public void ReactivatePlayer()
    {

        StartCoroutine(followOrb.MoveDummyToPlayerPosition(golfingScript.playerGO));
    }
}
