using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTransTEMP : MonoBehaviour
{

    private void DestroyAfterBallSunk()
    {
        EventManagerNorth.StopListening("GolfBallSunk", DestroyAfterBallSunk);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManagerNorth.StartListening("GolfBallSunk", DestroyAfterBallSunk);
    }

}
