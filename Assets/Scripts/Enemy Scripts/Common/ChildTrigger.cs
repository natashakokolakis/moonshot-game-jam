using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    EnemyAttacks parentAttack;

    private void Awake()
    {
        parentAttack = GetComponentInParent<EnemyAttacks>();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        parentAttack.OnChildTriggerEnter(other);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        parentAttack.OnChildCollisionEnter(collision);
    }
}
