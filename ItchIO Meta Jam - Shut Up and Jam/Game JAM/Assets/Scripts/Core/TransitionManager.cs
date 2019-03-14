using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public GameObject TransitionObject;
    public GameObject Foreground;
    public float TransitionTime = 0.5f;

    private Image m_ForegroundImage;

    public void Initialize()
    {
        TransitionObject.SetActive(true);
        Foreground.SetActive(false);

        m_ForegroundImage = Foreground.GetComponent<Image>();
    }

    public void SetState(bool flag)
    {
        if (flag)
        {
            Foreground.SetActive(true);
            m_ForegroundImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            Foreground.SetActive(false);
            m_ForegroundImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    public IEnumerator FadeIn()
    {
        StopCoroutine(FadeOut());

        Foreground.SetActive(true);

        float transitionDelta = 0.0f;
        while (transitionDelta < TransitionTime)
        {
            m_ForegroundImage.color = Color.Lerp(new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 0.0f, 0.0f, 1.0f), transitionDelta / TransitionTime);
            transitionDelta += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());

        Foreground.SetActive(true);

        float transitionDelta = 0.0f;
        while (transitionDelta < TransitionTime)
        {
            m_ForegroundImage.color = Color.Lerp(new Color(0.0f, 0.0f, 0.0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.0f), transitionDelta / TransitionTime);
            transitionDelta += Time.deltaTime;
            yield return null;
        }

        Foreground.SetActive(false);
    }
}
