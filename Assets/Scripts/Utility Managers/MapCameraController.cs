using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    #region Variables and Dependencies

    private CinemachineVirtualCamera thisCamera;
    private Transform playerTrans;
    private Vector3 moveDirection;
    private PlayerMoonGolfController playerMoonGolfController;
    private float mapMoveSpeed = 20f;
    private Vector3 cameraPosOffset = new Vector3(0, 30, 0);
    private GameObject mapModeCanvas;


    // for zoom controls
    private float defaultOrthoSize = 15;
    private float maxOrthSize = 20;
    private float minOrthSize = 10;
    public float currentOrthSize;

    public float zoomSpeed = 1f;


    //To keep gameobject in the center of camera view
    private Vector3 screenCentre = new Vector3(0.5f, 0.5f, 0);
    private Camera mainCamera;
    private bool cameraTransitionOver = false;

    public FMODUnity.StudioEventEmitter mapUiClick;

    [FMODUnity.EventRef]
    public string MapZoomInEvent = "";

    [FMODUnity.EventRef]
    public string MapZoomOutEvent = "";


    #endregion



    private void Awake()
    {
        mapUiClick = GetComponent<FMODUnity.StudioEventEmitter>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        currentOrthSize = defaultOrthoSize;
        mapModeCanvas = transform.Find("MapModeCanvas").gameObject;
        thisCamera = GetComponent<CinemachineVirtualCamera>();
        playerMoonGolfController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoonGolfController>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

    }

    IEnumerator WaitforCameraTransition()
    {
        yield return new WaitForSeconds(1);
        cameraTransitionOver = true;        
    }

    public void TurnOnMapMode()
    {
        this.transform.position = playerTrans.position + cameraPosOffset;
        thisCamera.m_Lens.OrthographicSize = defaultOrthoSize;
        thisCamera.Priority = 13;
        mapModeCanvas.SetActive(true);
        //EventManagerNorth.TriggerEvent("ToggleGolfMode");
        StartCoroutine(WaitforCameraTransition());
    }

    private void OnEnable()
    {
        TurnOnMapMode();

        if (playerMoonGolfController.gameObject.activeSelf == true)
        FMODUnity.RuntimeManager.PlayOneShot(MapZoomInEvent);
    }

    public void TurnOffMapMode()
    {
        if (playerMoonGolfController.gameObject.activeSelf == true)
            FMODUnity.RuntimeManager.PlayOneShot(MapZoomOutEvent);

        thisCamera.Priority = 9;
        mapModeCanvas.SetActive(false);
        if (playerMoonGolfController.gameObject.activeSelf)
        {
            moveDirection = Vector3.zero;
            playerMoonGolfController.isInAOE = false;
            EventManagerNorth.TriggerEvent("ToggleGolfMode");
        }
        cameraTransitionOver = false;
        enabled = (false);
    }

    private void HandleInput()
    {
        moveDirection = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };

        if (Input.GetKeyDown(KeyCode.Tab))
            TurnOffMapMode();

        if (Input.GetKey(KeyCode.Q))
        {
            DecreaseZoom();
        }

        if (Input.GetKey(KeyCode.E))
        {
            IncreaseZoom();
        }

    }

    private void IncreaseZoom()
    {
        thisCamera.m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
        if (thisCamera.m_Lens.OrthographicSize > maxOrthSize)
            thisCamera.m_Lens.OrthographicSize = maxOrthSize;
    }

    private void DecreaseZoom()
    {
        thisCamera.m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
        if (thisCamera.m_Lens.OrthographicSize < minOrthSize)
            thisCamera.m_Lens.OrthographicSize = minOrthSize;
    }

    private void MoveCamera()
    {
        if (moveDirection.magnitude > 0)
            mapUiClick.Play();
        transform.position += moveDirection * mapMoveSpeed * Time.deltaTime;
    }

    private void MoveTransformToCentre()
    {
        transform.position = mainCamera.ViewportToWorldPoint(screenCentre);

    }

    private void Update()
    {
        HandleInput();
        MoveCamera();
    }

    private void FixedUpdate()
    {
        if (cameraTransitionOver)
        MoveTransformToCentre();
    }

}
