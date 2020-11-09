using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallAttack : MonoBehaviour
{
    public Rigidbody golfBallRB;
    public float basePower = .03f;
    public void ShootGolfBall(float golfPower, Vector3 direction)
    {
        golfBallRB.AddForce(direction * golfPower * basePower, ForceMode.Impulse);
    }

    void OnEnable()
    {

    }

    private void Awake()
    {
        golfBallRB = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
}
