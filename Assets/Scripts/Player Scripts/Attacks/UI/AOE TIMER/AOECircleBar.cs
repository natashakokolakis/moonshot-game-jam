using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AOECircleBar : MonoBehaviour
{
    public AOEAttackHandler aoeHandler;
    private Image circleImage;

    private void Awake()
    {
        circleImage = GetComponent<Image>();
    }

    public IEnumerator CountDownAOETimer()
    {
        float maxTime = aoeHandler.maxSpecialTimer;

        while (aoeHandler.currentTime > 0)
        {
            circleImage.fillAmount = aoeHandler.currentTime / maxTime;

            yield return new WaitForEndOfFrame();
        }

        
        yield return null;
    }

}
