using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;

public class PushbackCollider : MonoBehaviour
{
    BossAttack bossAttack;
    int meleeDamage;

    private void Awake()
    {
        bossAttack = GetComponentInParent<BossAttack>();
        meleeDamage = bossAttack.meleeDamage;
        StartCoroutine(DeactivateSelf());
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
            bossAttack.DealDamage(meleeDamage);
        }
    }

    IEnumerator DeactivateSelf()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
