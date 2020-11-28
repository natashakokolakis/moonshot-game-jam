using System.Collections;
using System.Collections.Generic;
using ECM.Controllers;
using UnityEngine;

public sealed class ChaseBehaviourPrefab : BaseAgentController
{
    [HideInInspector] public GameObject chaseTarget;
    /*[HideInInspector]*/ public bool bossCanRotate = true;

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
        chaseTarget = GameObject.FindGameObjectWithTag("Player");
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

    private void BossRotateTowardsPlayer()
    {
        if (gameObject.CompareTag("Boss") && bossCanRotate)
            RotateTowards(chaseTarget.transform.position - transform.position);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        BossRotateTowardsPlayer();
    }

}
