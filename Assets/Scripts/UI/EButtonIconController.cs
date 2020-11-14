using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButtonIconController : MonoBehaviour
{
    public Transform eButtonPosition;
    public IconBounce eButtonObject;
    public Transform orbPosition;
    
    CapsuleCollider interactableCollider;

    

    private void Awake()
    {
        interactableCollider = GetComponent<CapsuleCollider>();
        orbPosition = this.transform;
    }

    public void disableIndicator()
    {
        eButtonObject.TurnOffIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eButtonPosition.position = orbPosition.position + new Vector3(0, 4, -12);
            eButtonObject.SetUpIndicator();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            disableIndicator();
        }
    }

}
