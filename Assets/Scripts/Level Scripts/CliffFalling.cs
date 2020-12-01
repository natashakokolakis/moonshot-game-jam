using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffFalling : MonoBehaviour
{
    Animator animate;

    private void Awake()
    {
        animate = GetComponent<Animator>();
    }

    public void StartFalling()
    {
        animate.SetTrigger("Fall");
    }

    public void DestroySelf()
    {
        Destroy(gameObject, 1);
    }
}
