using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StrokeCounter : MonoBehaviour
{
    [HideInInspector] public int strokeNumber = 0;
    [HideInInspector] public int par = 4;
    [HideInInspector] public TextMeshProUGUI strokeCounterText;

    void Start()
    {
        strokeCounterText = GameObject.Find("Stroke Counter").GetComponent<TextMeshProUGUI>();
        strokeCounterText.text = strokeNumber + "/" + par;
    }

    public void IncreaseStroke()
    {
        strokeNumber++;
        strokeCounterText.text = strokeNumber + "/" + par;
    }

    public void UpdatePar(int newPar)
    {
        par = newPar;
        strokeNumber = 0;
        strokeCounterText.text = strokeNumber + "/" + par;
    }
}
