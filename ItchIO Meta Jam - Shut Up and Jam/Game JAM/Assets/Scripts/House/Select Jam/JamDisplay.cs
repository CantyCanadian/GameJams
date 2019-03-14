using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamDisplay : MonoBehaviour
{
    public GameObject DisplayObject;

    public Text Title;
    public Text Description;
    public Text Teams;
    public Text Attemps;
    public Text Wins;
    public Text First;
    public Text Second;
    public Text Third;
    public GameObject DoorPrize;
    public DifficultyStars Stars;

    private bool m_Force = false;
    private JamButton m_Ref = null;
    private SelectJamLinker.Jam m_Jam;

    public void ShowDisplay(SelectJamLinker.Jam Jam, bool Force, JamButton ButtonRef = null)
    {
        if (m_Force && !Force)
        {
            return;
        }

        if (m_Force && Force && m_Ref != null)
        {
            m_Ref.LostControl();
            m_Ref = null;
        }

        DisplayObject.SetActive(true);

        DataManager dm = GameManager.Instance.Data;

        Title.text = dm.GetGameplayLoc(Jam.NameKey);
        Description.text = dm.GetGameplayLoc(Jam.DescriptionKey);
        Teams.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_TEAMS") + Jam.Participants.ToString();
        Attemps.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_ATTEMPTS") + Jam.Attempts.ToString();
        Wins.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_WINS") + Jam.Wins.ToString();
        First.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_FIRST") + Jam.FirstPlacePrice.ToString() + "$";
        Second.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_SECOND") + Jam.SecondPlacePrice.ToString() + "$";
        Third.text = dm.GetGameplayLoc("GAMEPLAY_JAMSELECT_THIRD") + Jam.ThirdPlacePrice.ToString() + "$";
        DoorPrize.SetActive(Jam.DoorPrizePrice != 0.0f);
        Stars.SetDifficulty(Jam.Difficulty);

        m_Ref = ButtonRef;
        m_Force = Force;
        m_Jam = Jam;
    }

    public void HideDisplay(bool Force = false)
    {
        if (m_Force && !Force)
        {
            return;
        }

        m_Force = false;

        if (m_Ref != null)
        {
            m_Ref.LostControl();
            m_Ref = null;
        }

        DisplayObject.SetActive(false);
    }

    public bool GetIsForced()
    {
        return m_Force;
    }

    public SelectJamLinker.Jam GetSelectedJam()
    {
        return m_Jam;
    }
}
