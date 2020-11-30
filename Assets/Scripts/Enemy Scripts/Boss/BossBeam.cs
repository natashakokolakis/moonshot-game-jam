using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    ChaseBehaviourPrefab chaseBehaviour;
    BossAttack bossAttack;

    private void OnEnable()
    {
        chaseBehaviour = GetComponentInParent<ChaseBehaviourPrefab>();
        bossAttack = GetComponentInParent<BossAttack>();
        StartCoroutine(BeamOff(3f));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(bossAttack.ultimateDamage);
        }
    }

    IEnumerator BeamOff(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
    /*public void TurnBeamOff()
    {
        gameObject.SetActive(false);
    }*/
}