using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    public ENEMY_STATE state;
    public float attackDelay = 1f;

    [HideInInspector] public float attackRange;

    [SerializeField] private bool onCooldown = false;

    private ChaseBehaviourPrefab chaseBehaviour;
    private EnemyHealth enemyHealth;
    private float enemySpeed;
    private Animator anim;
    private float playerDistance;
    private GameObject player;
    private Rigidbody rb;

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
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponentInChildren<Animator>(); 
        state = ENEMY_STATE.Chase;
        enemySpeed = chaseBehaviour.speed;
        player = GameObject.FindGameObjectWithTag("Player");
        attackRange = chaseBehaviour.brakingDistance;
        rb = GetComponent<Rigidbody>();
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
        if (enemyHealth.currentHealth <= 0)
        {
            StopAllCoroutines();
        }
        //chaseBehaviour.speed = enemySpeed; 
        rb.WakeUp();

        //execute chase
        while (state == ENEMY_STATE.Chase)
        {
            playerDistance = (player.transform.position - transform.position).magnitude;
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
        //chaseBehaviour.speed = 0;
        rb.velocity = Vector3.zero;
        rb.Sleep();

        //execute attack
        //while (state == ENEMY_STATE.Attack)
        //{
            //maybe get rid of the cooldown check here, just send enemy into chase state
            //after attack until cooldown is over
            //if (playerDistance <= attackRange && !onCooldown)
            //{
            anim.SetTrigger("isAttacking");

            onCooldown = true;
        //damage and code gets enabled from within animation event

        while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                yield return null;
            }

        //}
        
        StartCoroutine(StartCooldown());

        //if (playerDistance > attackRange)
        //{
        //    state = ENEMY_STATE.Chase;
        //    yield break;
        //}
        //}
        //yield return null;
        //}
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
