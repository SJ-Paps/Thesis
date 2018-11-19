using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneCrossFade : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer blackFade;

    [SerializeField]
    private float velocity;

    private bool isShowing;
    private bool isHiding;

    public void ShowCrossFade(Action callback)
    {
        isShowing = true;
        CoroutineManager.GetInstance().StartCoroutine(CrossFadeCoroutine(true, callback));
    }

    public void HideCrossFade(Action callback)
    {
        isHiding = true;
        CoroutineManager.GetInstance().StartCoroutine(CrossFadeCoroutine(false, callback));
    }

    private IEnumerator CrossFadeCoroutine(bool toShow, Action callback)
    {
        if (toShow)
        {
            while(isHiding)
            {
                yield return null;
            }

            while (blackFade.color.a > 0)
            {
                blackFade.color = new Color(blackFade.color.r, blackFade.color.g, blackFade.color.b, blackFade.color.a - (velocity * Time.fixedDeltaTime));

                Debug.Log(blackFade.color.a);
                yield return null;
            }

            isShowing = false;
        }
        else
        {
            while(isShowing)
            {
                yield return null;
            }

            while (blackFade.color.a < 1)
            {
                blackFade.color = new Color(blackFade.color.r, blackFade.color.g, blackFade.color.b, blackFade.color.a + (velocity * Time.fixedDeltaTime));

                Debug.Log(blackFade.color.a);

                yield return null;
            }

            isHiding = false;
        }

        if(callback != null)
        {
            callback();
        }
    }
}
