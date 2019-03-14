using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public string DefaultLanguage = "ASK";
    public int DefaultAspectRatio = 2;  //3:4
    public int DefaultResolution = 1;   //1024x768

    public string Language { set { m_Language = value; SaveData(); } get { return m_Language; } }
    public int AspectRatio { set { m_AspectRatio = value; SaveData(); } get { return m_AspectRatio; } }
    public int Resolution { set { m_Resolution = value; SaveData(); } get { return m_Resolution; } }

    private string m_Language;
    private int m_AspectRatio;
    private int m_Resolution;

    private string m_LanguageKey = "GAMELANGUAGE";
    private string m_AspectRatioKey = "ASPECTRATIO";
    private string m_ResolutionKey = "RESOLUTION";

    public void Initialize()
    {
        LoadData();
    }

	public void LoadData()
    {
        m_Language = PlayerPrefs.GetString(m_LanguageKey, DefaultLanguage);
        m_AspectRatio = PlayerPrefs.GetInt(m_AspectRatioKey, DefaultAspectRatio);
        m_Resolution = PlayerPrefs.GetInt(m_ResolutionKey, DefaultResolution);
    }

    private void SaveData()
    {
        PlayerPrefs.SetString(m_LanguageKey, m_Language);
        PlayerPrefs.SetInt(m_AspectRatioKey, m_AspectRatio);
        PlayerPrefs.SetInt(m_ResolutionKey, m_Resolution);
    }
}
