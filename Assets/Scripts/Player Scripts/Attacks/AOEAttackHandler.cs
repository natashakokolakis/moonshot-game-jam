using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AOEAttackHandler : MonoBehaviour
{
    #region Variables

    public float maxSpecialTimer = 5;
    public LayerMask enemyLayer;
    public float currentTime = 0f;
    public List<GameObject> enemiesTargeted = new List<GameObject>();
    public GameObject targetIcon;
    private PlayerMoonGolfController playerMoonGolfController;
    private PlayerAnimations animate;

    public float aoeCooldown = 4;
    private AOECircleBar aoeCircleTimer;
    private float timer;
    private Slider aoeSlider;
    private Tween aoeCooldownEnd;
    public bool coolingDown = false;

    private MeshRenderer ballRender;
    private TrailRenderer trailRenderer;
    public float ballSpeed = 15f;
    public int aoeDamage = 4;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private Vector3 targetImageOffset = new Vector3(0, 3, -4);
    private Quaternion targetImageRotation = Quaternion.Euler(new Vector3(30, 0, 0));

    #endregion

    private void Awake()
    {
        playerMoonGolfController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoonGolfController>();
        ballRender = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        animate = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimations>();
        aoeSlider = GameObject.Find("AOE Slider").GetComponent<Slider>();
        aoeCooldownEnd = aoeSlider.DOValue(0, 1).SetAutoKill(false).SetLoops(0, LoopType.Restart);
        aoeCircleTimer = GetComponentInChildren<AOECircleBar>();
    }

    public IEnumerator AOESpecial()
    {
        currentTime = maxSpecialTimer;
        aoeCircleTimer.enabled = true;
        StartCoroutine(aoeCircleTimer.CountDownAOETimer());
        EventManagerNorth.TriggerEvent("ToggleGolfMode");
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/MapZoomIn", transform.position);


        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, enemyLayer.value))
            {
                if (!enemiesTargeted.Contains(hitInfo.transform.gameObject))
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Golf Related/Ball Impact", transform.position);
                    enemiesTargeted.Add(hitInfo.transform.gameObject);
                    Vector3 targetPosition = hitInfo.transform.position + targetImageOffset;
                    Instantiate(targetIcon, targetPosition, targetImageRotation);

                }
            }

            yield return waitForEndOfFrame;

        }

        currentTime = 0f;
        aoeCircleTimer.enabled = false;

        if (enemiesTargeted.Count < 1)
        {
            animate.CancelAiming();

            EventManagerNorth.TriggerEvent("ToggleGolfMode");

            yield return waitForFixedUpdate;
            playerMoonGolfController.isInAOE = !playerMoonGolfController.isInAOE;

            yield break;
        }

        animate.RangedAttack(1, 1);
        StartCoroutine(StartTimer(aoeCooldown));
        StartCoroutine(AOECooldown());
        //playerMoonGolfController.ShootAttackBall();

        //StartCoroutine(MoveBallToTargets());

        yield return waitForEndOfFrame;
    }

    public IEnumerator MoveBallToTargets()
    {
        ballRender.enabled = true;
        trailRenderer.enabled = true;

        Vector3 startingPos = transform.position;
        Vector3 previousPos = startingPos;
        Vector3 offset = new Vector3(0, 1.5f, 0);

        int maxNumbTargeted = enemiesTargeted.Count;

        int i = 0;

        while (i < maxNumbTargeted)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemiesTargeted[i].transform.position + offset, ballSpeed * Time.deltaTime);

            if (transform.position == enemiesTargeted[i].transform.position + offset)
            {
                //Plays ball hit sound.
                FMODUnity.RuntimeManager.PlayOneShot("event:/Golf Related/Ball Impact", transform.position);
                i++;
            }
            yield return waitForFixedUpdate;
        }

        ballRender.enabled = false;
        trailRenderer.enabled = false;

        transform.localPosition = Vector3.zero;

        EventManagerNorth.TriggerEvent("ToggleGolfMode");

        yield return waitForFixedUpdate;
        playerMoonGolfController.isInAOE = !playerMoonGolfController.isInAOE;

        for (int a = 0; a < maxNumbTargeted; a++)
        {
            // Push Away -- enemiesTargeted[a].GetComponent<EnemyHealth>().TakeDamage(aoeDamage, startingPos);
            enemiesTargeted[a].GetComponent<EnemyHealth>().TakeDamage(aoeDamage, previousPos);
            previousPos = enemiesTargeted[a].transform.position;
        }

        enemiesTargeted.Clear();

        //Plays effect end sound
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/MapZoomOut", transform.position);

        yield return null;
    }

    IEnumerator StartTimer(float maxTime)
    {
        timer = maxTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        yield return null;
    }

    IEnumerator AOECooldown()
    {
        //block aoe attack
        coolingDown = true;

        aoeSlider.value = 1;
        yield return new WaitForSeconds((aoeCooldown - 1));
        aoeCooldownEnd.Restart();
        aoeCooldownEnd.Play();
        yield return new WaitForSeconds(1);

        //return aoe attack controls
        coolingDown = false;
    }
}
