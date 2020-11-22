using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class OrbGolfingScript : MonoBehaviour
{
    #region Variables and Dependencies

    // Components
    public Rigidbody golfBallRB;
    public SphereCollider golfBallCollider;
    public TrailRenderer trailRenderer;
    public CapsuleCollider interactableCollider;
    private GameObject playerGO;
    public EButtonIconController eButtonIndicator;
    public StrokeCounter strokeCounter;

    public Transform orbDummyPlayer;

    public FollowOrb playerIndicators;

    // Cameras
    public CinemachineVirtualCamera vCam;
    public MapCameraController mapCamera;

    // Turns on golf mode
    public bool isGolfing = false;

    // Stats
    public float basePower = 1f;
    public bool isStopped = true;

    // Cooldown to check when ball has stopped moving
    public float velCheckTimer = 0f;
    public float velCheckTimerStart = 1f;

    // For aiming
    public LayerMask groundMask = 1;
    public Vector3 attackDirection = Vector3.zero;

    public float golfPowerRate = 1f;
    public float golfPower = 0f;
    public float golfPowerMin = 0f;
    public float golfPowerMAX = 33f;

    #endregion

    private void Start()
    {
        playerGO = GameObject.Find("ECM_Player");
        vCam = GameObject.Find("OrbCinemaCam").GetComponent<CinemachineVirtualCamera>();
        mapCamera = GameObject.Find("Map Camera").GetComponent<MapCameraController>();

    }

    // Rotates player while aiming
    public void Rotate(Vector3 direction, float angularSpeed, bool onlyLateral = true)
    {
        if (onlyLateral)
            direction = Vector3.ProjectOnPlane(direction, transform.up);

        if (direction.sqrMagnitude < 0.0001f)
            return;

        var targetRotation = Quaternion.LookRotation(direction, transform.up);
        var newRotation = Quaternion.Slerp(golfBallRB.rotation, targetRotation,
            angularSpeed * Mathf.Deg2Rad * Time.deltaTime);

        golfBallRB.MoveRotation(newRotation);
    }

    public void SetUpGolfMode()
    {
        playerIndicators.SetUpAimLineAndPlayerModel();
        isGolfing = true;
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
        //mapCamera.Priority = 11;
        mapCamera.enabled = true;
        vCam.Priority = 11;
        vCam.Follow = transform;
        vCam.LookAt = transform;
        eButtonIndicator.disableIndicator();
        golfPowerRate = Mathf.Abs(golfPowerRate);
        golfPower = 0;
    }

    public void ShootGolfBall(float golfForce, Vector3 direction)
    {
        //mapCamera.Priority = 9;
        mapCamera.TurnOffMapMode();
        vCam.Priority = 11;

        strokeCounter.IncreaseStroke();

        playerIndicators.TurnOffAimLine();
        playerIndicators.StartCoroutine(playerIndicators.MoveDummyToPlayerPosition(playerGO));
        trailRenderer.Clear();

        golfBallRB.velocity = Vector3.zero;
        direction = direction * golfForce * basePower;

        golfBallRB.isKinematic = false;
        golfBallRB.AddForce(direction, ForceMode.VelocityChange);
        golfPower = 0f;
        isStopped = false;

    }

    public void AimGolfBall()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
            return;

        attackDirection = Vector3.ProjectOnPlane(hitInfo.point - transform.position, transform.up).normalized;
        Rotate(attackDirection, 1500, false);


        golfPower += golfPowerRate * Time.deltaTime;

        if (golfPower >= golfPowerMAX)
            golfPowerRate = -golfPowerRate;

        if (golfPower <= 0)
            golfPowerRate = -golfPowerRate;
    }

    public void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootGolfBall(golfPower, attackDirection);
        }

        if (Input.GetKeyDown(KeyCode.Tab) & !mapCamera.enabled)
            mapCamera.enabled = true;
    }

    #region Monobehaviours

    private void Awake()
    {
        golfBallRB = this.GetComponent<Rigidbody>();
        golfBallCollider = this.GetComponent<SphereCollider>();
        trailRenderer = this.GetComponent<TrailRenderer>();
        interactableCollider = this.GetComponent<CapsuleCollider>();
        eButtonIndicator = GetComponent<EButtonIconController>();

        strokeCounter = GameObject.Find("Stroke Counter").GetComponent<StrokeCounter>();

        golfBallRB.maxAngularVelocity = 100f;
    }

    private void FixedUpdate()
    {
        if (isStopped)
            return;

        // Waits to check ball velocity
        if (velCheckTimer >= velCheckTimerStart & !isGolfing)
        {
            // When is below a certain velocity, it is forced to stop
            if (golfBallRB.velocity.magnitude < 0.06f)
            {
                isStopped = true;
                velCheckTimer = 0f;
                golfBallRB.isKinematic = true;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
                //isGolfing = false;
                playerIndicators.TurnOffPlayerModel();
                playerGO.SetActive(true);
                //isGolfing = false;
                //playerGO.transform.position = playerIndicators.orbPlayerModel.transform.position - new Vector3(0, 1, 0);
                playerGO.transform.rotation = playerIndicators.orbPlayerModel.transform.rotation;
                vCam.Priority = 9;
                EventManagerNorth.TriggerEvent("ToggleGolfMode");
            }
        }

        velCheckTimer += Time.deltaTime;

    }

    private void Update()
    {
        if (isStopped & isGolfing)
        {
            AimGolfBall();
            HandleInput();
            playerIndicators.RotateToFollowAim();
        }

    }

    #endregion
}
