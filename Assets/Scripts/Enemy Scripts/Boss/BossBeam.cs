using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    public int damage = 4;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
