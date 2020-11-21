using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    public BoxCollider meleeHitbox;
    public int meleeDamage = 1;
    private Transform playerTransform;
    private MMFeedbacks hitEnemyFeedback;

    private void Awake()
    {
        playerTransform = this.transform.parent.parent.transform;
        hitEnemyFeedback = GetComponent<MMFeedbacks>();
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemyFeedback.PlayFeedbacks();
            other.GetComponent<EnemyHealth>().TakeDamage(meleeDamage, playerTransform.position);
        }
    }
}
