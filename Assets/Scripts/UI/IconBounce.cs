using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IconBounce : MonoBehaviour
{
    Tween subtleFloat;
    Tween stretchScale;
    Tween fadeImage;
    Sequence endSequence;
    SpriteRenderer imageToTween;

    // Start is called before the first frame update
    void Start()
    {
        imageToTween = GetComponent<SpriteRenderer>();
        subtleFloat = transform.DOMoveY(4.5f, .5f).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo);
        stretchScale = transform.DOScale(.7f, .5f).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo);
        fadeImage = imageToTween.DOFade(0, .5f).From().SetAutoKill(false);
        endSequence.Append(subtleFloat.Pause()).Append(stretchScale.Pause());
    }

    public void SetUpIndicator()
    {
        subtleFloat.Play();
        stretchScale.Play();
        fadeImage.Restart();
    }

    

    public void TurnOffIndicator()
    {
        fadeImage.PlayBackwards();
    }
}
