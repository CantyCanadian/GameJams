using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageLinker : MonoBehaviour
{
    public GameObject LanguageObject;

    private bool m_MoveForward = false;

    public void Initialize()
    {
        LanguageObject.SetActive(false);
    }

    public IEnumerator LanguageLoop()
    {
        LanguageObject.SetActive(true);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        yield return new WaitUntil(() => m_MoveForward);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

        LanguageObject.SetActive(false);
    }

    public void OnEnglishSelected()
    {
        GameManager.Instance.Settings.Language = "ENGLISH";
        m_MoveForward = true;
    }

    public void OnFrenchSelected()
    {
        GameManager.Instance.Settings.Language = "FRENCH";
        m_MoveForward = true;
    }
}
