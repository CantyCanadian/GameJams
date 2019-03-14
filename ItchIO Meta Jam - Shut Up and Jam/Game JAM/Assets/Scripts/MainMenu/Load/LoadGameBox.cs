using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameBox : MonoBehaviour
{
    public Text SaveName;
    public Text Money;
    public Text JamCount;
    public Text JamWins;

    private string m_Money;
    private string m_Count;
    private string m_Wins;

    public void SetData(string name, int money, int count, int wins)
    {
        SaveName.text = name;

        m_Money = money.ToString();
        m_Count = count.ToString();
        m_Wins = wins.ToString();
    }

    public void LoadData()
    {
        DataManager dm = GameManager.Instance.Data;

        Money.text = dm.GetGameplayLoc("GAMEPLAY_LOADGAME_MONEY") + m_Money;
        JamCount.text = dm.GetGameplayLoc("GAMEPLAY_LOADGAME_JAMSPLAYED") + m_Count;
        JamWins.text = dm.GetGameplayLoc("GAMEPLAY_LOADGAME_JAMSWON") + m_Wins;
    }

    public void SetTransform(GameObject target, GameObject parent)
    {
        transform.SetParent(parent.transform);

        transform.position = target.transform.position;
        transform.localScale = target.transform.localScale;
    }
}
