using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    public GameObject SplashObject;
    
    public GameObject[] SplashScreens;
    public float TransitionTime = 0.5f;
    public float SplashTime = 3.0f;

    private bool m_Done = false;

    public void Initialize()
    {
        SplashObject.SetActive(false);
        SplashScreens.DoOnAll((obj1) => { obj1.SetActive(false); return obj1; });
    }

    public IEnumerator PlayLoop()
    {
        Coroutine main = StartCoroutine(PlayLoopMain());
        Coroutine skip = StartCoroutine(PlayLoopSkip());

        yield return new WaitUntil(() => m_Done);

        StopCoroutine(main);
        StopCoroutine(skip);

        SplashObject.SetActive(false);
        SplashScreens.DoOnAll((obj1) => { obj1.SetActive(false); return obj1; });

        GameManager.Instance.Transition.SetState(true);
    }

    public IEnumerator PlayLoopMain()
    {
        StartCoroutine(PlayLoopSkip());

        SplashObject.SetActive(true);
        SplashScreens.DoOnAll((obj1) => { obj1.SetActive(false); return obj1; });

        for (int i = 0; i < SplashScreens.Length; i++)
        {
            SplashScreens[i].SetActive(true);

            yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());
            yield return new WaitForSeconds(SplashTime);
            yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

            SplashScreens[i].SetActive(false);
        }
        
        SplashObject.SetActive(false);

        m_Done = true;
    }

    public IEnumerator PlayLoopSkip()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                break;
            }

            yield return null;
        }

        m_Done = true;
    }
}