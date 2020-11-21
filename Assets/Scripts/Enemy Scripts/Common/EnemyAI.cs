using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public ENEMY_STATE state;

    private void Awake()
    {
        state = ENEMY_STATE.Chase;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyFSM());
    }

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator Chase()
    {
        //enter chase state

        //execute chase
        while (state == ENEMY_STATE.Chase)
        {

            yield return null;
        }

        //exit chase state

    }

    IEnumerator Attack()
    {
        //enter attack state
        //chasebehaviourprefab speed = 0

        //execute attack
        while (state == ENEMY_STATE.Attack)
        {
            //trigger attack animation, damage and code gets enabled from within animation event

            yield return null;
        }

        //exit attack state
        //chasebehaviourprefab speed = previous speed
    }

    public enum ENEMY_STATE
    {
        Chase, 
        Attack
    }

}
