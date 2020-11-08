using System.Collections;
using System.Collections.Generic;
using ECM.Controllers;
using UnityEngine;

public sealed class ChaseBehaviourPrefab : BaseAgentController
{
    public GameObject chaseTarget;
    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
            pause = !pause;

        if (chaseTarget != null)
        agent.SetDestination(chaseTarget.transform.position);
    }

    public void OnEnable()
    {
        chaseTarget = GameObject.Find("ECM_Player");
        
    }

}
