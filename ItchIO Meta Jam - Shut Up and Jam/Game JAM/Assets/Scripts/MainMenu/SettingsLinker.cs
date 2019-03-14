using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLinker : MonoBehaviour
{
    public GameObject SettingsObject;
    public GameObject Warning;
    public Dropdown AspectRatio;
    public Dropdown Resolution;

    private bool m_Exit = false;
    private string m_OriginalLanguage;

    private List<List<Dropdown.OptionData>> m_ResolutionOptions;
    private Dictionary<string, Vector2Int> m_Resolutions;
    private int m_AspectRatio;
    private int m_Resolution;

    public void Initialize()
    {
        SettingsObject.SetActive(false);
        Warning.SetActive(false);

        m_ResolutionOptions = new List<List<Dropdown.OptionData>>();
        m_ResolutionOptions.Add(new List<Dropdown.OptionData>());
        m_ResolutionOptions.Add(new List<Dropdown.OptionData>());
        m_ResolutionOptions.Add(new List<Dropdown.OptionData>());

        m_ResolutionOptions[0].Add(new Dropdown.OptionData("1280x720"));
        m_ResolutionOptions[0].Add(new Dropdown.OptionData("1600x900"));
        m_ResolutionOptions[0].Add(new Dropdown.OptionData("1920x1080"));
        m_ResolutionOptions[0].Add(new Dropdown.OptionData("3840x2160"));

        m_ResolutionOptions[1].Add(new Dropdown.OptionData("1280x800"));
        m_ResolutionOptions[1].Add(new Dropdown.OptionData("1440x900"));
        m_ResolutionOptions[1].Add(new Dropdown.OptionData("1680x1050"));
        m_ResolutionOptions[1].Add(new Dropdown.OptionData("1920x1200"));
        m_ResolutionOptions[1].Add(new Dropdown.OptionData("2560x1600"));

        m_ResolutionOptions[2].Add(new Dropdown.OptionData("800x600"));
        m_ResolutionOptions[2].Add(new Dropdown.OptionData("1024x768"));
        m_ResolutionOptions[2].Add(new Dropdown.OptionData("1280x960"));
        m_ResolutionOptions[2].Add(new Dropdown.OptionData("1400x1050"));
        m_ResolutionOptions[2].Add(new Dropdown.OptionData("1600x1200"));

        m_Resolutions = new Dictionary<string, Vector2Int>();

        foreach (List<Dropdown.OptionData> ldo in m_ResolutionOptions)
        {
            foreach (Dropdown.OptionData dod in ldo)
            {
                int xIndex = dod.text.IndexOf('x');
                string pre = dod.text.Substring(0, xIndex);
                string post = dod.text.Substring(xIndex + 1, dod.text.Length - xIndex - 1);
                m_Resolutions.Add(dod.text, new Vector2Int(int.Parse(pre), int.Parse(post)));
            }
        }
    }

    public IEnumerator SettingsLoop()
    {
        AspectRatio.value = GameManager.Instance.Settings.AspectRatio;
        Resolution.options = m_ResolutionOptions[AspectRatio.value];
        Resolution.value = GameManager.Instance.Settings.Resolution;

        GameManager.Instance.Background.ShowScreenBackground();

        SettingsObject.SetActive(true);

        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        switch (GameManager.Instance.Settings.Language)
        {
            case "ENGLISH":
                m_OriginalLanguage = "ENGLISH";
                break;

            case "FRENCH":
                m_OriginalLanguage = "FRENCH";
                break;
        }

        yield return new WaitUntil(() => m_Exit);
        m_Exit = false;

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

        SettingsObject.SetActive(false);
    }

    public void OnLanguageChanged(Dropdown origin)
    {
        switch(origin.value)
        {
            case 0:
                GameManager.Instance.Settings.Language = "ENGLISH";
                Warning.SetActive(m_OriginalLanguage != "ENGLISH");
                break;

            case 1:
                GameManager.Instance.Settings.Language = "FRENCH";
                Warning.SetActive(m_OriginalLanguage != "FRENCH");
                break;
        }
    }

    public void OnAspectRatioChanged(Dropdown origin)
    {
        Resolution.options = m_ResolutionOptions[origin.value];
        Resolution.value = 0;

        m_AspectRatio = origin.value;
        m_Resolution = 0;
    }

    public void OnResolutionChanged(Dropdown origin)
    {
        m_Resolution = origin.value;
    }

    public void OnApplyResolutionPressed()
    {
        GameManager.Instance.Settings.AspectRatio = m_AspectRatio;
        GameManager.Instance.Settings.Resolution = m_Resolution;

        Vector2Int newResolution = m_Resolutions[m_ResolutionOptions[m_AspectRatio][m_Resolution].text];
        Screen.SetResolution(newResolution.x, newResolution.y, true);
    }

    public void OnExitPressed()
    {
        m_Exit = true;
    }
}
