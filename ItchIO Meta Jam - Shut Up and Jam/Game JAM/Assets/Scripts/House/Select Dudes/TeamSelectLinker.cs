using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectLinker : MonoBehaviour
{
    public GameObject TeamSelectObject;

    public ProfileLinker Profile;

    public GameObject Subwindows;

    private bool m_Next = false;
    private bool m_Subwindow = false;
    private bool m_IsOpenProfile = false;

    public void Initialize()
    {
        TeamSelectObject.SetActive(false);
        Subwindows.SetActive(false);
    }

    public IEnumerator TeamSelectLoop()
    {
        TeamSelectObject.SetActive(true);
        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        while (m_Next == false)
        {
            if (m_Subwindow)
            {
                Subwindows.SetActive(true);

                if (m_IsOpenProfile)
                {
                    yield return StartCoroutine(Profile.ProfileLoop());
                }
                else
                {

                }

                m_Subwindow = false;

                Subwindows.SetActive(false);
            }

            yield return new WaitUntil(() => m_Next || m_Subwindow);
        }

        m_Next = false;

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());
        TeamSelectObject.SetActive(false);
    }

    public void OnOpenProfileClicked()
    {
        m_Subwindow = true;
        m_IsOpenProfile = true;
    }
}
