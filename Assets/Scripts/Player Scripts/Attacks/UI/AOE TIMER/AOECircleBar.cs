using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AOECircleBar : MonoBehaviour
{
    private AOEAttackHandler aoeHandler;
    private Image circleImage;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        aoeHandler = GameObject.Find("AOE Handler").GetComponent<AOEAttackHandler>();
        circleImage = GetComponent<Image>();
    }

    public IEnumerator CountDownAOETimer()
    {
        float maxTime = aoeHandler.maxSpecialTimer;

        while (aoeHandler.currentTime > 0)
        {
            circleImage.fillAmount = aoeHandler.currentTime / maxTime;

            yield return waitForEndOfFrame;
        }
        circleImage.fillAmount = 0;
        yield return null;
    }

}
