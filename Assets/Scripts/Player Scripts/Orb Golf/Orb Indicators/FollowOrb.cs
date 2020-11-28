using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FollowOrb : MonoBehaviour
{
    public OrbGolfingScript orbController;
    public GameObject aimLine;
    public GameObject orbPlayerModel;
 

    public float dummySpeed = 4f;

    //original values == private Vector3 dummyStartPos = new Vector3(0, .75f, -1);
    private Vector3 dummyStartPos = new Vector3(0, .5f, -1);

    private Vector3 dummyRelativeY = new Vector3(0, .5f, 0);

    public void SetUpAimLineAndPlayerModel()
    {
        aimLine.SetActive(true);
        orbPlayerModel.transform.localPosition = dummyStartPos;
        orbPlayerModel.SetActive(true);
        transform.position = orbController.transform.position;
        transform.rotation = orbController.transform.rotation;
    }

    public void TurnOffAimLine()
    {
        aimLine.SetActive(false);
    }

    public void TurnOffPlayerModel()
    {
        orbPlayerModel.SetActive(false);
    }

    public void MoveDummyToPlayer(Vector3 target)
    {
        orbPlayerModel.transform.position = Vector3.MoveTowards(orbPlayerModel.transform.position, target + Vector3.up, dummySpeed * Time.deltaTime);
    }

    public IEnumerator MoveDummyToPlayerPosition(GameObject target)
    {
        bool isLinedUp = false;

        Vector3 targetPosition = target.transform.localPosition + dummyRelativeY;

        while (!isLinedUp)
        {
            orbPlayerModel.transform.position = Vector3.MoveTowards(orbPlayerModel.transform.position, targetPosition, dummySpeed * Time.deltaTime);
            if ((orbPlayerModel.transform.position.x - targetPosition.x) < 0.1f && (orbPlayerModel.transform.position.z - targetPosition.z) < 0.1f)
                isLinedUp = true;

            yield return null;
        }

        orbController.isGolfing = false;

        yield break;
    }

    public void RotateToFollowAim()
    {
        transform.rotation = orbController.transform.rotation;
    }

}
