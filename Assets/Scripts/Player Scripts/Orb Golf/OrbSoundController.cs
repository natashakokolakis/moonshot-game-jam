using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSoundController : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string GolfBallImpact = "";

   /* [FMODUnity.EventRef]
    public string GolfBallRoll = "";*/

    FMOD.Studio.EventInstance golfImpactEvent;

    //FMOD.Studio.EventInstance golfBallRoll;

    private LayerMask groundMask = 1;


    private void Start()
    {
       

    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactPower = Mathf.Clamp(collision.relativeVelocity.magnitude / 10.0f, 0, 5);

        /*        if (collision.gameObject.layer == groundMask)
                {
                    golfBallRoll = FMODUnity.RuntimeManager.CreateInstance(GolfBallImpact);
                    golfBallRoll.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

                    //golfImpactEvent.setParameterByName("BallVolume", impactPower);
                    Debug.Log("start roll");
                    golfBallRoll.start();
                    golfBallRoll.release();
                }
                else*/

        if (!(collision.gameObject.layer == groundMask))
            PlayImpactSound(impactPower);

    }

    private void PlayImpactSound(float audioLevel)
    {
        golfImpactEvent = FMODUnity.RuntimeManager.CreateInstance(GolfBallImpact);
        golfImpactEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        golfImpactEvent.setParameterByName("BallVolume", audioLevel);

        golfImpactEvent.start();
        golfImpactEvent.release();
    }

}
