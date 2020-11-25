using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButtonIconController : MonoBehaviour
{
    public GameObject eButton;
    Transform eButtonPosition;
    IconBounce eButtonBounce;
    Transform orbPosition;
    
    //CapsuleCollider interactableCollider;

    private void Awake()
    {
        eButtonPosition = eButton.transform;
        eButtonBounce = eButton.GetComponent<IconBounce>();
        //interactableCollider = GetComponent<CapsuleCollider>();
        orbPosition = this.transform;
    }

    public void disableIndicator()
    {
        eButtonBounce.TurnOffIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eButtonPosition.position = orbPosition.position + new Vector3(0, 4, -12);
            eButtonBounce.SetUpIndicator();
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
