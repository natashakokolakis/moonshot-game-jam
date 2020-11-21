using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    public ENEMY_STATE state;
    public float attackRange;

    private ChaseBehaviourPrefab chaseBehaviour;
    private float enemySpeed;
    private Animator anim;

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
        //enter chase state

        //execute chase
        while (state == ENEMY_STATE.Chase)
        {
            //check if player within range, if it is switch state
            //if (chaseBehaviour.chaseTarget.transform.position - transform.position < attackRange)

            yield return null;
        }

        //exit chase state

    }

    IEnumerator Attack()
    {
        //enter attack state
        //stop moving for attack 
        chaseBehaviour.speed = 0;
        anim.SetTrigger("isAttacking");

        //execute attack
        //trigger attack animation, damage and code gets enabled from within animation event
        //set state = ENEMY_STATE.Chase; after attack complete

        while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") )
        {
            yield return null;
        }

        //exit attack state
        //reset enemy speed
        chaseBehaviour.speed = enemySpeed;
    }
    #endregion

    bool AnimationComplete (string animationTag)
    {
        var state = anim.GetCurrentAnimatorStateInfo(0);

        return state.IsTag(animationTag) && state.normalizedTime >= 1;
    }
}
