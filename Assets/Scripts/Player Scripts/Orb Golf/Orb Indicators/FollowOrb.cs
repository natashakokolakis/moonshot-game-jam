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
    private Vector3 dummyStartPos = new Vector3(0, .75f, -1);

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

        while (!isLinedUp)
        {
            orbPlayerModel.transform.position = Vector3.MoveTowards(orbPlayerModel.transform.position, target.transform.position + Vector3.up, dummySpeed * Time.deltaTime);
            if (orbPlayerModel.transform.position == target.transform.position + Vector3.up)
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
