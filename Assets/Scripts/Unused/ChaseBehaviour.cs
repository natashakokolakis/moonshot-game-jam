using System.Collections;
using System.Collections.Generic;
using ECM.Controllers;
using UnityEngine;

public sealed class ChaseBehaviour : BaseAgentController
{
    public GameObject chaseTarget;
    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
            pause = !pause;

        if (chaseTarget != null)
        agent.SetDestination(chaseTarget.transform.position);
    }


}
