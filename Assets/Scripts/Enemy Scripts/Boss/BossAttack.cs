using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

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
    public Vector3 minionSpawnRange = new Vector3(10, 1, 10);
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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
        chaseBehaviour = GetComponentInParent<ChaseBehaviourPrefab>();
        bossBeam = chargeBeam.GetComponent<BossBeam>();
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

        if (isAttacking == false && onCooldown == false && playerHealth.currentHealth > 0)
        {
            /*if (isEnraged)
            {
                enragedMultiplier = 1.5f;
            }*/

            isAttacking = true;
            onCooldown = true;

            int num = Random.Range(0, 5);


            if (num < 1)
            {
                //play charging animation
                //ChargedAttack();
                animate.SetTrigger("ChargeAttack");
            }
            else if (num < 3 && num >=1)
            {
                //play summon animation
                //SummonMinions();
                animate.SetTrigger("SummonMinions");
            }
            else
            {
                //Debug.DrawRay(bossParent.transform.position, (bossParent.transform.position - player.transform.position).normalized * meleeAttackRange, Color.yellow, 5);
                if (Vector3.Magnitude(player.transform.position - bossParent.transform.position) <= meleeAttackRange)
                {
                    //play melee animation
                    //MeleeAttack();
                    
                    animate.SetTrigger("MeleeAttack");
                }
                else
                {
                    //player ranged attack animation
                    //RangedAttack();
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
        animate.enabled = !animate.enabled;
    }

    public void ChargedAttack()
    {
        //modify so that boss charges before activating beam

        chaseBehaviour.bossCanRotate = false;
        chargeBeam.SetActive(true);

        //StartCoroutine(AttackDuration(10f, 3f));
    }

    public void SummonMinions()
    {


        //summon 5 minions at random positions
        for (int i = 0; i < 5; i++)
        {
            minion = minions[Random.Range(0, minions.Count - 1)];
            //NavMeshHit hit;
            //NavMesh.SamplePosition(minionSpawnPoint.position + randomPosition, out hit, 1, 1);
            //randomPosition = hit.position;

            Vector3 randomPosition = new Vector3(Random.Range(-minionSpawnRange.x, minionSpawnRange.x), 0, Random.Range(-minionSpawnRange.y, minionSpawnRange.y));

            Instantiate(minion, randomPosition, minionSpawnPoint.rotation);
            NavMeshAgent minionNav = minion.GetComponent<NavMeshAgent>();

            if (!minionNav.isOnNavMesh)
            {
                NavMeshHit hit;
                NavMesh.FindClosestEdge(randomPosition, out hit, NavMesh.AllAreas);
            }
        }

        //StartCoroutine(AttackDuration(2f, 7f));
    }

    public void MeleeAttack()
    {

        DealDamage(meleeDamage);

        //StartCoroutine(AttackDuration(1f, 3f));
    }

    //not called yet, swap in when ready and make sure collider gets deactivated
    public void MeleeStomp()
    {
        

        sphereCollider.enabled = true;
    }

    public void RangedAttack()
    {

        Instantiate(projectile, projectileOrigin.position, transform.rotation);

        //StartCoroutine(AttackDuration(1f, 3f));
    }

    public void MeleeComplete()
    {
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
        bossBeam.TurnBeamOff();
        chaseBehaviour.bossCanRotate = true;
        StartCoroutine(NextAttackDelay(ultimateCooldown));
    }

    //remove this method after animations get coded in
    IEnumerator AttackDuration(float duration, float minTimeBetweenAttack)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
        StartCoroutine(NextAttackDelay(minTimeBetweenAttack));
    }

    IEnumerator NextAttackDelay(float minTimeBetweenAttack)
    {
        //instead of running two coroutines, call this at end of each attack animation
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(minionSpawnPoint.position, minionSpawnRange);

    }
}
