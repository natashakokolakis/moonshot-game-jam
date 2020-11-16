using System.Collections;
using System.Collections.Generic;
using ECM.Controllers;
using UnityEngine;

public sealed class ChaseBehaviourPrefab : BaseAgentController
{
    public GameObject chaseTarget;
    protected override void HandleInput()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //    pause = !pause;


        if (chaseTarget != null)
        agent.SetDestination(chaseTarget.transform.position);
 
    }

    private void PauseWhenGolfing()
    {
        pause = !pause;
    }

    public void OnEnable()
    {
        chaseTarget = GameObject.Find("ECM_Player");
        EventManagerNorth.StartListening("ToggleGolfMode", PauseWhenGolfing);
    }

    public void OnDisable()
    {
        EventManagerNorth.StopListening("ToggleGolfMode", PauseWhenGolfing);
    }

    public void OnDestroy()
    {
        EventManagerNorth.StopListening("ToggleGolfMode", PauseWhenGolfing);
    }

}
