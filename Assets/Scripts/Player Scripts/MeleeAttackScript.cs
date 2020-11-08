using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    public BoxCollider meleeHitbox;
    public int meleeDamage = 5;

    private void Awake()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(meleeDamage);
        }
    }

    void Update()
    {
        
    }
}
