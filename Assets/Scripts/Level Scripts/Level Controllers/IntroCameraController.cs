using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{
    private CinemachineDollyCart dollyCart;
    public float dollyPositionOnTrack = 0f;
    public float dollyPositionFinal;
    private CinemachineVirtualCamera introCamera;

    private PlayerMoonGolfController playerController;

    // Start is called before the first frame update
    void Awake()
    {
        dollyCart = transform.Find("IntroDollyCart").GetComponent<CinemachineDollyCart>();
        dollyCart.m_Position = dollyPositionOnTrack;
        introCamera = transform.Find("IntroCam").GetComponent<CinemachineVirtualCamera>();
        dollyPositionFinal = transform.Find("IntroDollyTrack").GetComponent<CinemachineSmoothPath>().PathLength;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoonGolfController>();
    }

    private void SwitchToIntroCam()
    {
        introCamera.Priority = 15;
    }

    private void StartDollyCart()
    {
        dollyCart.m_Position = 0;
        dollyCart.enabled = true;

    }

    public void PlayLevelPreview()
    {
        
        SwitchToIntroCam();
        StartDollyCart();
    }

    private bool CheckIfFinishedMove()
    {
        dollyPositionOnTrack = dollyCart.m_Position;
        if (dollyPositionOnTrack == dollyPositionFinal)
            return true;
        else
            return false;
    }

    private void TurnOffIntroCamAndUnPausePlayer()
    {
        introCamera.Priority = 9;
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
        playerController.isInAOE = false;
        enabled = false;
    }

    private void FixedUpdate()
    {
        if (CheckIfFinishedMove())
        {
            TurnOffIntroCamAndUnPausePlayer();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TurnOffIntroCamAndUnPausePlayer();
    }

}
