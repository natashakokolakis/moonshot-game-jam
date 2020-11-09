using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    public BoxCollider meleeHitbox;
    public int meleeDamage = 1;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = this.transform.parent.parent.transform;
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(meleeDamage, playerTransform.position);
        }
    }
}
