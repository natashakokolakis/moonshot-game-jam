using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StrokeCounter : MonoBehaviour
{
    [HideInInspector] public int strokeNumber = 0;
    [HideInInspector] public int par = 4;
    [HideInInspector] public int strokeTotal = 0;
    [HideInInspector] public TextMeshProUGUI strokeCounterText;
    [HideInInspector] public TextMeshProUGUI strokeTotalText;

    void Start()
    {
        strokeCounterText = GameObject.Find("Stroke Counter").GetComponent<TextMeshProUGUI>();
        strokeCounterText.text = strokeNumber + "/" + par;
        strokeTotalText = GameObject.Find("Stroke Total").GetComponent<TextMeshProUGUI>();
        strokeTotalText.text = (strokeTotal + strokeNumber).ToString();
    }

    public void IncreaseStroke()
    {
        strokeNumber++;
        strokeCounterText.text = strokeNumber + "/" + par;
        strokeTotalText.text = (strokeTotal + strokeNumber).ToString();
    }

    public void UpdateLevelStrokes(int newPar)
    {
        par = newPar;
        strokeTotal += strokeNumber;
        strokeNumber = 0;
        strokeCounterText.text = strokeNumber + "/" + par;
    }
}
