using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    PlayerMoonGolfController playerController;
    OrbGolfingScript golfingScript;
    FollowOrb followOrb;

    [FMODUnity.EventRef]
    public string GolfBallHitEvent = "";

    [FMODUnity.EventRef]
    public string PuttHitEvent = "";

    [FMODUnity.EventRef]
    public string SwingClubEvent = "";

    [FMODUnity.EventRef]
    public string FootstepEvent = "";

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

    public void PlayGolfBallSound()
    {
        FMOD.Studio.EventInstance golfBAllHitSound = FMODUnity.RuntimeManager.CreateInstance(GolfBallHitEvent);
        int randomBallSoundNumber = Random.Range(0, 5);

        golfBAllHitSound.setParameterByName("GolfBallSoundSelection", randomBallSoundNumber, true);
        golfBAllHitSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        golfBAllHitSound.start();
        golfBAllHitSound.release();
    }

    public void PlayPuttSound()
    {
        FMOD.Studio.EventInstance puttHitSound = FMODUnity.RuntimeManager.CreateInstance(PuttHitEvent);
        int randomBallSoundNumber = Random.Range(0, 5);

        puttHitSound.setParameterByName("GolfBallSoundSelection", randomBallSoundNumber, true);
        puttHitSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        puttHitSound.start();
        puttHitSound.release();
    }

    public void PlaySwingSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SwingClubEvent, transform.position);
    }


    public void PlayFootStepSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(FootstepEvent, transform.position);
    }

}
