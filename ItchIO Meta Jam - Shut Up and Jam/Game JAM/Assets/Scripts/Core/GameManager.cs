using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TransitionManager Transition;
    public SplashManager Splash;
    public DataManager Data;
    public SettingsManager Settings;
    public SaveManager Saving;
    public BackgroundManager Background;
    public PlayerManager Player;

    public MainMenuLinker MainMenuLink;
    public LoadGameLinker LoadGameLink;
    public LanguageLinker LanguageLink;
    public SettingsLinker SettingsLink;
    public SelectJamLinker SelectJamLink;
    public TeamSelectLinker TeamSelectLink;
    public ProfileLinker ProfileLink;

    public InputField NewGameName;

    public static GameManager Instance { get { return s_Instance; } }
    private static GameManager s_Instance = null;

    private IEnumerator GameLoop()
    {
        Data.LoadEarlyData();

        yield return StartCoroutine(LanguageLink.LanguageLoop());

        yield return StartCoroutine(Data.LoadData());

        if (Splash != null)
        {
            yield return StartCoroutine(Splash.PlayLoop());
        }

        bool exit = false;
        while(exit == false)
        {
            yield return StartCoroutine(MainMenuLink.MainMenuLoop());

            switch (MainMenuLink.GetResult())
            {
                case 0: //NEW GAME
                    yield return StartCoroutine(MainMenuLink.NewGameLoop());

                    if (MainMenuLink.GetResult() == 0) //START GAME
                    {
                        Saving.CreateNewSave(NewGameName.text);
                        yield return StartCoroutine(GameplayLoop());
                    }
                    break;

                case 1: //LOAD GAME
                    yield return StartCoroutine(LoadGameLink.LoadGameLoop());

                    string result = LoadGameLink.GetResult();
                    if (result != string.Empty)
                    {
                        Saving.Load(result);
                        yield return StartCoroutine(GameplayLoop());
                    }
                    break;

                case 2: //SETTINGS
                    yield return StartCoroutine(SettingsLink.SettingsLoop());
                    break;

                default:
                    exit = true;
                    break;
            }
        }
    }

    private IEnumerator GameplayLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SelectJamLink.SelectJamLoop());

            yield return StartCoroutine(TeamSelectLink.TeamSelectLoop());
        }
    }

	void Start ()
    {
		if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //Initalization Phase
        Transition.Initialize();
        Splash.Initialize();
        Data.Initialize();
        Settings.Initialize();
        Saving.Initialize();
        Background.Initialize();
        Player.Initialize();

        MainMenuLink.Initialize();
        LoadGameLink.Initialize();
        LanguageLink.Initialize();
        SettingsLink.Initialize();
        SelectJamLink.Initialize();
        TeamSelectLink.Initialize();
        ProfileLink.Initialize();

        StartCoroutine(GameLoop());
	}
}
