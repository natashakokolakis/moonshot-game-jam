using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StrokeCounterOld : MonoBehaviour
{

    public int strokeCount = 0;
    private TMP_Text strokeCountText;

    void Start()
    {
        strokeCountText = GetComponent<TMP_Text>();
        strokeCountText.text = strokeCount.ToString();
    }

    public void IncreaseStroke()
    {
        strokeCount++;
        strokeCountText.text = strokeCount.ToString();

    }

}
