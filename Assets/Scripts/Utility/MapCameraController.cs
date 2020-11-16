using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera thisCamera;
    private Transform playerTrans;
    private Vector3 moveDirection;
    private PlayerMoonGolfController playerMoonGolfController;
    private float mapMoveSpeed = 20f;

    private void Awake()
    {
        thisCamera = GetComponent<CinemachineVirtualCamera>();
        playerMoonGolfController = GameObject.Find("ECM_Player").GetComponent<PlayerMoonGolfController>();
        playerTrans = GameObject.Find("ECM_Player").transform;

    }

    public void TurnOnMapMode()
    {
        this.transform.position = playerTrans.position + new Vector3(0, 19, -35);
        thisCamera.Priority = 13;
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
    }

    private void OnEnable()
    {
        TurnOnMapMode();
    }

    public void TurnOffMapMode()
    {
        thisCamera.Priority = 9;
        moveDirection = Vector3.zero;
        playerMoonGolfController.isInAOE = false;
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
        gameObject.SetActive(false);
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

    }

    private void MoveCamera()
    {
        transform.position += moveDirection * mapMoveSpeed * Time.deltaTime;
    }

    private void Update()
    {
        HandleInput();
        MoveCamera();
    }

}
