using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetCurrentLevel : MonoBehaviour
{
    #region Variables and Dependencies

    #region Camera and Locator Settings
    private Transform thisLevelCameraBounds;
    private SetBoundingBox currentCameraBounds;

    private PointToOffScreenTarget holeLocator;
    private Transform thisLevelHole;

    private PointToOffScreenTarget gbLocator;
    private Transform thisLevelGB;
    #endregion

    #region Enemy Settings
    private GameObject enemyList;
    #endregion

    #endregion

    void Awake()
    {
        InitializeCameraAndLocatorSettings();
        InitializeEnemyGroup();


    }

    private void InitializeCameraAndLocatorSettings()
    {
        currentCameraBounds = GameObject.Find("CurrentBounds").GetComponent<SetBoundingBox>();
        thisLevelCameraBounds = transform.Find("---- CameraBounds ----");

        holeLocator = GameObject.Find("Map Camera").transform.Find("MapModeCanvas").transform.Find("Hole Location").GetComponentInChildren<PointToOffScreenTarget>();
        thisLevelHole = transform.Find("---- Golf ----").Find("GolfHoleCollider");

        gbLocator = GameObject.Find("GolfBall Locator").GetComponentInChildren<PointToOffScreenTarget>();
        thisLevelGB = transform.Find("---- Golf ----").Find("---- Orb Golf ----").Find("THE ORB");

    }

    private void InitializeEnemyGroup()
    {
        enemyList = transform.Find("---- Enemies ----").gameObject;
    }

    private void SetCameraConfinersAndLocatorTargets()
    {
        currentCameraBounds.ChangeBoundingBox(thisLevelCameraBounds);

        holeLocator.ChangeTargetTransform(thisLevelHole.transform);
        gbLocator.ChangeTargetTransform(thisLevelGB.transform);
    }

    private void TurnOnEnemies()
    {
        enemyList.SetActive(true);
    }

    private void SetUpLevel()
    {
        SetCameraConfinersAndLocatorTargets();
        TurnOnEnemies();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetUpLevel();
        }
    }

}
