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

    #region Level Gates
    private LevelGateController nextLevelGate;
    [Header("Place 'Next Level Gate' from previous level here")]
    public LevelGateController previousLevelGate;
    #endregion

    #region IntroCamera
    private IntroCameraController introCameraController;
    public bool ignoreIntroCameras = false;

    #endregion

    #region Player Controller
    private PlayerMoonGolfController playerController;

    #endregion

    #region TriggerCollider
    private BoxCollider startLevelTrigger;
    #endregion

    public PlayerHealth playerHealthController;

    #endregion

    // Need to set previous level gate in Scene

    #region Initializtions
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

    private void GetNextLevelGate()
    {
        nextLevelGate = transform.Find("Next Level Gate").GetComponent<LevelGateController>();
    }

    private void GetIntroCamera()
    {
        if (!ignoreIntroCameras)
            introCameraController = transform.Find("IntroCamera").GetComponent<IntroCameraController>();
    }

    private void GetPlayerComponents()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoonGolfController>();
    }

    private void GetTriggerCollider()
    {
        startLevelTrigger = GetComponent<BoxCollider>();
    }

    // Grabs all above components
    private void InitializeAll()
    {
        InitializeCameraAndLocatorSettings();
        InitializeEnemyGroup();
        GetNextLevelGate();
        GetIntroCamera();
        GetPlayerComponents();
        GetTriggerCollider();

        playerHealthController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

    }

    #endregion

    #region Setting Up Current Level
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
    
    private void SetUpLevelGates()
    {
        nextLevelGate.enabled = true;
        if (previousLevelGate.isActiveAndEnabled)
        previousLevelGate.StopBacktracking();
    }

    private void PlayLevelIntroAndPausePlayer()
    {
        introCameraController.enabled = true;
        introCameraController.PlayLevelPreview();
        playerController.isInAOE = true;
        playerController.moveDirection = Vector3.zero;
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
    }

    private void SetUpLevel()
    {
        SetCameraConfinersAndLocatorTargets();
        TurnOnEnemies();
        SetUpLevelGates();
        if (!ignoreIntroCameras)
            PlayLevelIntroAndPausePlayer();
        startLevelTrigger.enabled = false;

        playerHealthController.currentHealth = playerHealthController.maxHealth;

    }

    #endregion

    #region Monobehaviours

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetUpLevel();
        }
    }

    void Awake()
    {
        InitializeAll();
    }
    #endregion
}
