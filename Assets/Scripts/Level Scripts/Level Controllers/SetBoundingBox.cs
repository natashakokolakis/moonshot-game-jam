using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoundingBox : MonoBehaviour
{
    private BoxCollider currentBoxCollider;
    public Transform level1Colider;


    private void Awake()
    {
        transform.position = level1Colider.position;
        transform.localScale = level1Colider.localScale;
    }

    public void ChangeBoundingBox(Transform newBoundingBox)
    {
        transform.position = newBoundingBox.position;
        transform.localScale = newBoundingBox.localScale;
    }

}
