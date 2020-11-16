using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackHandler : MonoBehaviour
{

    public float maxSpecialTimer = 5;
    public LayerMask enemyLayer;
    public float currentTime = 0f;
    public List<GameObject> enemiesTargeted = new List<GameObject>();
    public GameObject targetIcon;
    public PlayerMoonGolfController playerMoonGolfController;

    public AOECircleBar aoeCircleTimer;

    private MeshRenderer ballRender;
    private TrailRenderer trailRenderer;
    public float ballSpeed = 15f;
    public int aoeDamage = 4;

    private void Awake()
    {
        playerMoonGolfController = GameObject.Find("ECM_Player").GetComponent<PlayerMoonGolfController>();
        ballRender = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public IEnumerator AOESpecial ()
    {
        currentTime = maxSpecialTimer;
        aoeCircleTimer.enabled = true;
        StartCoroutine(aoeCircleTimer.CountDownAOETimer());
        EventManagerNorth.TriggerEvent("ToggleGolfMode");

        while (currentTime > 0 )
        {
            currentTime -= Time.deltaTime;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, enemyLayer.value))
            {
                if (!enemiesTargeted.Contains(hitInfo.transform.gameObject))
                {
                    enemiesTargeted.Add(hitInfo.transform.gameObject);
                    Vector3 targetPosition = hitInfo.transform.position + new Vector3(0,3,-4);
                    Instantiate(targetIcon, targetPosition, Quaternion.Euler(new Vector3(30, 0, 0)));
                    Debug.Log("hit enemy");
                }
            }

            yield return new WaitForEndOfFrame();

        }

        currentTime = 0f;
        aoeCircleTimer.enabled = false;

        StartCoroutine(MoveBallToTargets());


        

        yield return new WaitForEndOfFrame();
    }

    IEnumerator MoveBallToTargets()
    {
        ballRender.enabled = true;
        trailRenderer.enabled = true;

        Vector3 startingPos = transform.position;

        int maxNumbTargeted = enemiesTargeted.Count;

        int i = 0;

        while (i < maxNumbTargeted)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemiesTargeted[i].transform.position + new Vector3(0, 1.5f, 0), ballSpeed * Time.deltaTime);

            if (transform.position == enemiesTargeted[i].transform.position + new Vector3(0, 1.5f, 0))
            {
                i++;
                Debug.Log("Hit Enemy");
            }
            yield return new WaitForFixedUpdate();
        }


        ballRender.enabled = false;
        trailRenderer.enabled = false;

        transform.localPosition = Vector3.zero;

        EventManagerNorth.TriggerEvent("ToggleGolfMode");

        yield return new WaitForFixedUpdate();
        playerMoonGolfController.isInAOE = !playerMoonGolfController.isInAOE;

        for (int a = 0; a < maxNumbTargeted; a++)
        {
            enemiesTargeted[a].GetComponent<EnemyHealth>().TakeDamage(aoeDamage, startingPos);
        }


        enemiesTargeted.Clear();


        yield return null;
    }

/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(AOESpecial());
        }
    }*/

}
