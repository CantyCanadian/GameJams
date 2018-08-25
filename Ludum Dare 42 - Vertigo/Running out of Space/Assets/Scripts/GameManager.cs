using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Camera OtherCam;
    public GameObject Timer;
    public GameObject MainMenu;
    public Text LastScore;
    public Text TopScore;
    public DayNight DayNightCycle;

    private Spawner m_Spawner;
    public GameObject m_Player;

	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        m_Spawner = GetComponent<Spawner>();
        OtherCam.enabled = true;
        Timer.SetActive(false);
        MainMenu.SetActive(true);

        LastScore.text = "Last Score : " + PlayerPrefs.GetInt("LAST_SCORE", 0);
        TopScore.text = "Top Score : " + PlayerPrefs.GetInt("TOP_SCORE", 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        Timer.SetActive(true);
        m_Player.SetActive(true);
        OtherCam.enabled = false;
        m_Spawner.Initialize();
        MainMenu.SetActive(false);
        DayNightCycle.Initialize();
    }

    public void GameOver()
    {
        int lastScore = int.Parse(Timer.GetComponent<Text>().text) + 2;
        int topScore = PlayerPrefs.GetInt("TOP_SCORE", 0);

        PlayerPrefs.SetInt("LAST_SCORE", lastScore);
        if (lastScore > topScore)
        {
            PlayerPrefs.SetInt("TOP_SCORE", lastScore);
        }

        SceneManager.LoadScene(0);
    }
}
