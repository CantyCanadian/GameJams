using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLinker : MonoBehaviour
{
    public GameObject MainMenuParent;
    public GameObject MainMenu;
    public GameObject NewGameMenu;

    private int m_ChoiceSelected = -1;

    public void Initialize()
    {
        MainMenuParent.SetActive(true);
        MainMenu.SetActive(false);
        NewGameMenu.SetActive(false);
    }

    public int GetResult()
    {
        return m_ChoiceSelected;
    }

    public IEnumerator MainMenuLoop()
    {
        m_ChoiceSelected = -1;

        GameManager.Instance.Background.ShowScreenBackground();

        MainMenu.SetActive(true);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        yield return new WaitUntil(() => m_ChoiceSelected > -1);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

        MainMenu.SetActive(false);
    }

    public IEnumerator NewGameLoop()
    {
        m_ChoiceSelected = -1;

        GameManager.Instance.Background.ShowScreenBackground();

        NewGameMenu.SetActive(true);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        yield return new WaitUntil(() => m_ChoiceSelected > -1);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

        NewGameMenu.SetActive(false);
    }

    public void OnNewGamePressed()
    {
        m_ChoiceSelected = 0;
    }

    public void OnLoadGamePressed()
    {
        m_ChoiceSelected = 1;
    }

    public void OnSettingsPressed()
    {
        m_ChoiceSelected = 2;
    }

    public void OnExitGamePressed()
    {
        m_ChoiceSelected = 3;
    }

    public void OnLetsGoPressed()
    {
        m_ChoiceSelected = 0;
    }

    public void OnNewGameBackPressed()
    {
        m_ChoiceSelected = 1;
    }
}
