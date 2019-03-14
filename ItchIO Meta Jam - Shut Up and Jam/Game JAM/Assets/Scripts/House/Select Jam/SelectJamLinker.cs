using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectJamLinker : MonoBehaviour, ISaveable
{
    public struct Jam
    {
        public enum JamType
        {
            General,
            Programming,
            Art,
            Audio
        }

        public Jam(string nameKey, int difficulty, string descriptionKey, JamType gameJamType, int participants, float firstPlace, float secondPlace, float thirdPlace, float participation)
        {
            Unlocked = false;

            NameKey = nameKey;
            Difficulty = difficulty;
            DescriptionKey = descriptionKey;
            GameJamType = gameJamType;

            Participants = participants;
            Attempts = 0;
            Wins = 0;

            FirstPlacePrice = firstPlace;
            SecondPlacePrice = secondPlace;
            ThirdPlacePrice = thirdPlace;
            DoorPrizePrice = participation;
        }

        public void LoadGame(bool unlocked, int attempts, int wins)
        {
            Unlocked = unlocked;
            Attempts = attempts;
            Wins = wins;
        }

        public bool Unlocked;

        public string NameKey;
        public int Difficulty;
        public string DescriptionKey;
        public JamType GameJamType;

        public int Participants;
        public int Attempts;
        public int Wins;

        public float FirstPlacePrice;
        public float SecondPlacePrice;
        public float ThirdPlacePrice;
        public float DoorPrizePrice;
    }

    public GameObject SelectJamObject;

    public GameObject DotContainer;
    public GameObject DotParent;

    public GameObject ChoiceContainer;
    public GameObject ChoiceParent;
    public JamDisplay JamDisplay;

    public Text JamGroupTitle;

    public Button NextButton;

    private List<DotContainer> m_Containers;
    private Dictionary<string, List<Jam>> m_Jams;
    private Dictionary<string, string> m_JamGroups;

    private List<GameObject> m_ChoiceButtons;

    private int m_Index = 1;
    private bool m_Next = false;
    private bool m_Change = false;

	public void Initialize()
    {
        SelectJamObject.SetActive(false);
        GameManager.Instance.Background.ShowBlackBackground();

        m_Jams = new Dictionary<string, List<Jam>>();
        m_JamGroups = new Dictionary<string, string>();
        m_Containers = new List<DotContainer>();
        m_ChoiceButtons = new List<GameObject>();

        List<Jam> globalJams = new List<Jam>();

        globalJams.Add(new Jam("GAMEPLAY_GLOBALJAM_GLOBALJAMNAME", 4, "GAMEPLAY_GLOBALJAM_GLOBALJAMDESC", Jam.JamType.General, 100, 200.0f, 50.0f, 50.0f, 0.0f));
        globalJams.Add(new Jam("GAMEPLAY_GLOBALJAM_METAJAMNAME", 3, "GAMEPLAY_GLOBALJAM_METAJAMDESC", Jam.JamType.General, 150, 150.0f, 50.0f, 25.0f, 0.0f));

        m_Jams.Add("Global Jams", globalJams);
        m_JamGroups.Add("Global Jams", "GAMEPLAY_GLOBALJAM");

        m_Containers.Add(Instantiate(DotContainer, DotParent.transform).GetComponent<DotContainer>());

        List<Jam> localJams = new List<Jam>();

        localJams.Add(new Jam("GAMEPLAY_LOCALJAM_COLLEGEJAMNAME", 2, "GAMEPLAY_LOCALJAM_COLLEGEJAMDESC", Jam.JamType.General, 10, 50.0f, 25.0f, 15.0f, 5.0f));
        localJams.Add(new Jam("GAMEPLAY_LOCALJAM_NGINJAMNAME", 1, "GAMEPLAY_LOCALJAM_NGINJAMDESC", Jam.JamType.Programming, 8, 40.0f, 20.0f, 10.0f, 0.0f));
        localJams.Add(new Jam("GAMEPLAY_LOCALJAM_MONTHLYARTJAMNAME", 1, "GAMEPLAY_LOCALJAM_MONTHLYARTJAMDESC", Jam.JamType.Art, 8, 40.0f, 20.0f, 10.0f, 0.0f));

        m_Jams.Add("Local Jams", localJams);
        m_JamGroups.Add("Local Jams", "GAMEPLAY_LOCALJAM");

        m_Containers.Add(Instantiate(DotContainer, DotParent.transform).GetComponent<DotContainer>());

        List<Jam> collegeJams = new List<Jam>();

        collegeJams.Add(new Jam("GAMEPLAY_COLLEGELEAGUE_ANDJAMNAME", 2, "GAMEPLAY_COLLEGELEAGUE_ANDJAMDESC", Jam.JamType.Programming, 25, 75.0f, 50.0f, 25.0f, 10.0f));
        collegeJams.Add(new Jam("GAMEPLAY_COLLEGELEAGUE_ARTSCHOOLNAME", 2, "GAMEPLAY_COLLEGELEAGUE_ARTSCHOOLDESC", Jam.JamType.Art, 20, 75.0f, 50.0f, 25.0f, 10.0f));
        collegeJams.Add(new Jam("GAMEPLAY_COLLEGELEAGUE_BEATZNAME", 2, "GAMEPLAY_COLLEGELEAGUE_BEATZDESC", Jam.JamType.Audio, 18, 85.0f, 55.0f, 40.0f, 10.0f));
        collegeJams.Add(new Jam("GAMEPLAY_COLLEGELEAGUE_TRICOLLEGENAME", 3, "GAMEPLAY_COLLEGELEAGUE_TRICOLLEGEDESC", Jam.JamType.General, 40, 120.0f, 80.0f, 40.0f, 25.0f));

        m_Jams.Add("College Jams", collegeJams);
        m_JamGroups.Add("College Jams", "GAMEPLAY_COLLEGELEAGUE");

        m_Containers.Add(Instantiate(DotContainer, DotParent.transform).GetComponent<DotContainer>());

        GameManager.Instance.Saving.RegisterSaveable(SaveManager.SaveOrder.Jams, this);
    }

    public IEnumerator SelectJamLoop()
    {
        Coroutine input = StartCoroutine(SelectJamInput());

        SelectJamObject.SetActive(true);
        SetJam();
        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        bool first = false;
        m_Index = 1;
        while (m_Next == false)
        {
            if (!first)
            {
                first = true;
            }
            else
            {
                SetJam();
            }

            yield return new WaitUntil(() => m_Change || m_Next);
            m_Change = false;
        }

        m_Next = false;

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());
        SelectJamObject.SetActive(false);

        StopCoroutine(input);
    }

    public IEnumerator SelectJamInput()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                OnLeftPressed();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                OnRightPressed();
            }

            if (Input.GetKeyDown(KeyCode.Return) && JamDisplay.GetIsForced())
            {
                OnNextPressed();
            }

            NextButton.interactable = JamDisplay.GetIsForced();
            yield return null;
        }
    }

    public string[] GetSaveableData()
    {
        List<string> save = new List<string>();

        foreach(KeyValuePair<string, List<Jam>> kvp in m_Jams)
        {
            foreach(Jam j in kvp.Value)
            {
                save.Add(j.Unlocked.ToString());
                save.Add(j.Attempts.ToString());
                save.Add(j.Wins.ToString());
            }
        }

        return save.ToArray();
    }

    public void SetSaveableData()
    {
        for(int i = 0; i < m_Jams["Local Jams"].Count; i++)
        {
            Jam j = m_Jams["Local Jams"][i];
            j.Unlocked = true;
            m_Jams["Local Jams"][i] = j;
        }

        for (int i = 0; i < m_Jams["Global Jams"].Count; i++)
        {
            Jam j = m_Jams["Global Jams"][i];
            j.Unlocked = true;
            m_Jams["Global Jams"][i] = j;
        }
    }

    public void SetSaveableData(string[] data)
    {
        int index = 0;

        foreach (KeyValuePair<string, List<Jam>> kvp in m_Jams)
        {
            foreach (Jam j in kvp.Value)
            {
                j.LoadGame(bool.Parse(data[index]), int.Parse(data[index + 1]), int.Parse(data[index + 2]));

                index += 3;
            }
        }
    }

    public void OnRightPressed()
    {
        if (m_Index + 1 >= m_Containers.Count)
        {
            return;
        }

        m_Index++;
        m_Change = true;
    }

    public void OnLeftPressed()
    {
        if (m_Index - 1 < 0)
        {
            return;
        }

        m_Index--;
        m_Change = true;
    }

    public void OnNextPressed()
    {
        JamDisplay.GetSelectedJam();
        m_Next = true;
    }

    private void SetJam()
    {
        JamDisplay.HideDisplay(true);

        for(int i = 0; i < m_ChoiceButtons.Count; i++)
        {
            Destroy(m_ChoiceButtons[i]);
        }

        m_ChoiceButtons.Clear();

        m_Containers.DoOnAll(new System.Func<DotContainer, DotContainer>((obj1) => { obj1.SetFlag(false); return obj1; }));
        m_Containers[m_Index].SetFlag(true);

        int index = 0;
        foreach(KeyValuePair<string, List<Jam>> kvp in m_Jams)
        {
            if (index != m_Index)
            {
                index++;
                continue;
            }

            JamGroupTitle.text = GameManager.Instance.Data.GetGameplayLoc(m_JamGroups[kvp.Key]);

            for(int i = 0; i < kvp.Value.Count; i++)
            {
                GameObject go = Instantiate(ChoiceContainer, ChoiceParent.transform);
                go.GetComponent<JamButton>().Initialize(kvp.Value[i], JamDisplay);
                m_ChoiceButtons.Add(go);
            }

            break;
        }
    }
}
