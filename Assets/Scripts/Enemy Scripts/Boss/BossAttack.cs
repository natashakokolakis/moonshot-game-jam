using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using ECM.Components;

public class BossAttack : MonoBehaviour
{
    /*IDEAS FOR POSSIBLE ATTACK/MECHANIC ALTERNATIVES
     * SUMMON - maybe change to be a list of enemies to summon at random
     * STOMP - do damage and push back everything (player/minions) within a radius. Use either a trigger to 
     *                  determine what's inside on activation or a reg collider that scales and pushes everything outward
     * CHARGE BEAM - either charge to shoot or takes a few sec to recover (breathing heavily), during which
     *                  time boss takes extra damage
     */

    GameObject minion;
    public List<GameObject> minions;

    public Transform minionSpawnPoint; //center point of where minions will spawn
    public Vector3 minionSpawnRange = new Vector3(8.5f, 1, 8.5f);
    public GameObject projectile;
    public Transform projectileOrigin;
    public GameObject chargeBeam;
    public float meleeAttackRange;
    public int meleeDamage;
    public int rangedDamage;
    public int ultimateDamage;
    public float meleeCooldown;
    public float rangedCooldown;
    public float spawnCooldown;
    public float ultimateCooldown;

    float enragedMultiplier = 1;
    [HideInInspector] public bool isEnraged = false;
    bool isAttacking = false;
    [HideInInspector] public bool onCooldown;
    string lastAttack; //set value to be name of last type, don't repeat special/ult too many times
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    SphereCollider sphereCollider;
    ChaseBehaviourPrefab chaseBehaviour;
    BossBeam bossBeam;
    Animator animate;
    GameObject bossParent;
    CharacterMovement enemyMovementController;

    PlayerMoonGolfController playerMoonGolfController;


    [FMODUnity.EventRef]
    public string BossMeleeEvent = "";

    [FMODUnity.EventRef]
    public string BossRangedEvent = "";


    [FMODUnity.EventRef]
    public string BossSummonEvent = "";

    [FMODUnity.EventRef]
    public string BossBeamEvent = "";

    [FMODUnity.EventRef]
    public string FireBallEvent = "";


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoonGolfController = player.GetComponent<PlayerMoonGolfController>();
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
        chaseBehaviour = GetComponentInParent<ChaseBehaviourPrefab>();
        bossBeam = chargeBeam.GetComponent<BossBeam>();
        enemyMovementController = GetComponentInParent<CharacterMovement>();
        meleeDamage = (int)(meleeDamage * enragedMultiplier);
        rangedDamage = (int)(rangedDamage * enragedMultiplier);
        ultimateDamage = (int)(ultimateDamage * enragedMultiplier);
        sphereCollider = GetComponent<SphereCollider>();
        animate = GetComponent<Animator>();
        bossParent = GameObject.FindGameObjectWithTag("Boss");
    }


    void FixedUpdate()
    {
        /*if (onCooldown = false && playerHealth.currentHealth <= 0)
        {
            animate.SetBool("PlayerDead", true);
            onCooldown = true;
        }*/

        if (isAttacking == false && onCooldown == false && playerHealth.currentHealth > 0 && !playerMoonGolfController.isInAOE)
        {
            if (isEnraged)
            {
                
                enragedMultiplier = 1.5f;
            }

            isAttacking = true;
            onCooldown = true;

            int num = Random.Range(0, 5);


            if (num < 1)
            {
                FMODUnity.RuntimeManager.PlayOneShot(BossBeamEvent, transform.position);
                animate.SetTrigger("ChargeAttack");
                chaseBehaviour.bossCanRotate = false;
            }
            else if (num < 3 && num >=1)
            {
                FMODUnity.RuntimeManager.PlayOneShot(BossSummonEvent, transform.position);
                animate.SetTrigger("SummonMinions");
            }
            else
            {
                if (Vector3.Magnitude(player.transform.position - bossParent.transform.position) <= meleeAttackRange)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(BossMeleeEvent, transform.position);
                    animate.SetTrigger("MeleeAttack");
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot(BossRangedEvent, transform.position);
                    animate.SetTrigger("RangedAttack");
                }
            }
        }
    }

    public void OnEnable()
    {
        EventManagerNorth.StartListening("ToggleGolfMode", PauseAnimation);
    }

    public void OnDisable()
    {
        EventManagerNorth.StopListening("ToggleGolfMode", PauseAnimation);
    }

    public void OnDestroy()
    {
        EventManagerNorth.StopListening("ToggleGolfMode", PauseAnimation);
    }

    void PauseAnimation()
    {
        //triggered during player special
        animate.enabled = !animate.enabled;
    }

    public void ChargedAttack()
    {
        chargeBeam.SetActive(true);
    }

    public void SummonMinions()
    {
        for (int i = 0; i < 4; i++)
        {
            minion = minions[Random.Range(0, minions.Count - 1)];
            Vector3 randomPosition = new Vector3(Random.Range(-minionSpawnRange.x, minionSpawnRange.x), 0, Random.Range(-minionSpawnRange.y, minionSpawnRange.y));

            Instantiate(minion, randomPosition + minionSpawnPoint.position, minionSpawnPoint.rotation);
            NavMeshAgent minionNav = minion.GetComponent<NavMeshAgent>();

            if (!minionNav.isOnNavMesh)
            {
                NavMeshHit hit;
                NavMesh.FindClosestEdge(randomPosition, out hit, NavMesh.AllAreas);
            }
        }
    }

    public void MeleePushback()
    {
        sphereCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //push them backwards from boss by a certain amount
            var travelDirection = (other.transform.position - transform.position).normalized;
            other.GetComponent<CharacterMovement>().ApplyForce(travelDirection * meleeDamage * 150, ForceMode.Impulse);
        }
        else if (other.CompareTag("Player"))
        {
            var travelDirection = (other.transform.position - transform.position).normalized;
            other.GetComponent<CharacterMovement>().ApplyForce(travelDirection * meleeDamage * 3000, ForceMode.Impulse);
            DealDamage(meleeDamage);
        }
    }

    public void RangedAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(FireBallEvent, transform.position);
        Instantiate(projectile, projectileOrigin.position, transform.rotation);
    }

    public void MeleeComplete()
    {
        sphereCollider.enabled = false;
        StartCoroutine(NextAttackDelay(meleeCooldown));
    }

    public void RangedComplete()
    {
        StartCoroutine(NextAttackDelay(rangedCooldown));
    }

    public void SpawnComplete()
    {
        StartCoroutine(NextAttackDelay(spawnCooldown));
    }

    public void UltimateComplete()
    {
        chaseBehaviour.bossCanRotate = true;
        StartCoroutine(NextAttackDelay(ultimateCooldown));
    }

    IEnumerator NextAttackDelay(float minTimeBetweenAttack)
    {
        isAttacking = false;
        yield return new WaitForSeconds(minTimeBetweenAttack);
        onCooldown = false;
    }

    public void DealDamage(int attackDamage)
    {
        playerHealth.TakeDamage(attackDamage);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //visualize minion spawn box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(minionSpawnPoint.position, minionSpawnRange);

    }
}
