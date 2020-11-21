using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    public ENEMY_STATE state;
    public float attackRange;
    public float attackDelay;

    private ChaseBehaviourPrefab chaseBehaviour;
    private float enemySpeed;
    private Animator anim;
    private bool onCooldown = false;
    private float playerDistance;

    #endregion


    #region enum
    public enum ENEMY_STATE
    {
        Chase,
        Attack
    }

    #endregion


    #region Unity Methods

    private void Awake()
    {
        chaseBehaviour = GetComponent<ChaseBehaviourPrefab>();
        anim = GetComponent<Animator>();
        state = ENEMY_STATE.Chase;
        enemySpeed = chaseBehaviour.speed;
        playerDistance = (chaseBehaviour.chaseTarget.transform.position - transform.position).magnitude;
    }

    void Start()
    {
        StartCoroutine(EnemyFSM());
    }
    #endregion


    #region Coroutines
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator Chase()
    {
        //enter chase state, make sure movement speed set
        chaseBehaviour.speed = enemySpeed;

        //execute chase
        while (state == ENEMY_STATE.Chase)
        {
            if (playerDistance <= attackRange && !onCooldown)
            {
                state = ENEMY_STATE.Attack;
                yield break; 
            }

            yield return null;
        }
    }

    IEnumerator Attack()
    {
        //enter attack state
        chaseBehaviour.speed = 0;

        //execute attack
        while (state == ENEMY_STATE.Attack)
        {
            anim.SetTrigger("isAttacking");
            //damage and code gets enabled from within animation event

            while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                yield return null;
            }
            onCooldown = true;
            StartCoroutine(StartCooldown());

            if (playerDistance > attackRange)
            {
                state = ENEMY_STATE.Chase;
                yield break;
            }
        }
        //exit attack state
        state = ENEMY_STATE.Chase;
        yield break;
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackDelay);
        onCooldown = false;
    }
    #endregion
}
