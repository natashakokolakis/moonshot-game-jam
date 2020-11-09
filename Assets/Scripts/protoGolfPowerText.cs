using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class protoGolfPowerText : MonoBehaviour
{
    public PlayerMoonGolfController playerController;
    public Text textReadout;
    public int golfInt;

    void Update()
    {
        golfInt = (int)playerController.golfPower;
        textReadout.text = golfInt.ToString();
    }
}
