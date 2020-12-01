using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckpoint : MonoBehaviour
{
    public GameObject enemyGroup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyGroup.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
