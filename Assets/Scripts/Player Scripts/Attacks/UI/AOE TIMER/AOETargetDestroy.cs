using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargetDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        EventManagerNorth.StartListening("ToggleGolfMode", DestroyImage);

    }

    private void DestroyImage()
    {
        EventManagerNorth.StopListening("ToggleGolfMode", DestroyImage);
        Object.Destroy(gameObject);

    }

}
