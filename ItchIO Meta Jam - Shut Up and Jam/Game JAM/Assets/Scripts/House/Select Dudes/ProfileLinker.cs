using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileLinker : MonoBehaviour
{
    public GameObject ProfileObject;

    public Text Name;
    public Text Gender;
    public Text Description;

    public Text Programming;
    public Text Debugging;
    public Text Creativity;
    public Text Art;
    public Text Music;
    public Text BugChance;

    public Text[] PersonalityName;
    public Text[] PersonalityDesc;
    public GameObject[] PersonalityBox;

    public Text Level;
    public Text ExpText;
    public RectTransform ExpMask;

    public DotContainer[] Dots;

    private int m_Index = 0;
    private float m_Width = 0;

    private bool m_Exit = false;
    private bool m_Update = false;

    public void Initialize()
    {
        ProfileObject.SetActive(false);
        m_Width = ExpMask.sizeDelta.x;
    }

    public IEnumerator ProfileLoop()
    {
        ProfileObject.SetActive(true);

        Dots[m_Index].SetFlag(true);

        while (!m_Exit)
        {
            SetData();

            yield return new WaitUntil(() => m_Exit || m_Update);
            m_Update = false;
        }

        m_Exit = false;

        ProfileObject.SetActive(true);
    }

    public void OnLeftClicked()
    {
        Dots[m_Index].SetFlag(false);
        m_Index--;

        if (m_Index < 0)
        {
            m_Index = 3;
        }

        Dots[m_Index].SetFlag(true);
        m_Update = true;
    }

    public void OnRightClicked()
    {
        Dots[m_Index].SetFlag(false);
        m_Index++;

        if (m_Index > 3)
        {
            m_Index = 0;
        }

        Dots[m_Index].SetFlag(true);
        m_Update = true;
    }

    public void OnExitClicked()
    {
        m_Exit = true;
    }

    private void SetData()
    {
        TeamMember[] team = GameManager.Instance.Player.GetTeam();

        if (team.Length != 4)
        {
            return;
        }

        TeamMember member = team[m_Index];

        Name.text = member.Name;
        Gender.text = member.GenderIsMale ? GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_TEAMSELECT_MALE") : GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_TEAMSELECT_FEMALE");
        Description.text = GameManager.Instance.Data.GetCharacterLoc(member.DescriptionKey);

        Programming.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_PROGRAMMING") + member.Programming;
        Debugging.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_DEBUGGING") + member.Debugging;
        Creativity.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_CREATIVITY") + member.Creativity;
        Art.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_ART") + member.Art;
        Music.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_MUSIC") + member.Audio;
        BugChance.text = GameManager.Instance.Data.GetGameplayLoc("GAMEPLAY_PROFILE_BUGCHANCE") + member.Bugging;

        PersonalityBox.DoOnAll(new System.Func<GameObject, GameObject>((obj) => { obj.SetActive(false); return obj; }));

        for (int i = 0; i < 3; i++)
        {
            TeamMember.PositivePersonality pp = member.PersonalityPositive[i];
            string result = member.GetPersonalityDescriptionKey(pp);

            if (result == "NULL")
            {
                PersonalityBox[i].SetActive(false);
            }
            else
            {
                PersonalityName[i].text = pp.ToString();
                PersonalityDesc[i].text = result;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int truei = i + 3;
            TeamMember.NegativePersonality np = member.PersonalityNegative[i];
            string result = member.GetPersonalityDescriptionKey(np);

            if (result == "NULL")
            {
                PersonalityBox[truei].SetActive(false);
            }
            else
            {
                PersonalityName[truei].text = np.ToString();
                PersonalityDesc[truei].text = result;
            }
        }

        Vector2 exp = new Vector2(member.CurrentExp, member.CalculateExp(member.Level));
        ExpText.text = exp.x.ToString() + "/" + exp.y.ToString();
        Level.text = "LV " + member.Level.ToString();
        ExpMask.sizeDelta = new Vector2(Mathf.Lerp(0, m_Width, exp.x / exp.y), ExpMask.sizeDelta.y);
    }
}
