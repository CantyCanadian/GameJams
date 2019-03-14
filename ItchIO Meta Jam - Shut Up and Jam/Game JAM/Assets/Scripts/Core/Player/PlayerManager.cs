using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveable
{
    public enum Items
    {
        UPS,
        CoffeeMachine,
        MechanicalKeyboard,
        DrawingTablet,
        MIDIKeyboard,
        ExtraLaptop,
        DesktopUpgrade,
        NginLicense,
        AdoughbeeLicense,
        BeatzLicense,
        EnergyDrink,
        Coffee,
        Chips,
        Granola,
        Count
    }

    private int m_Money;
    private int m_JamCount;
    private int m_JamWins;

    private TeamMember[] m_Team;
    private Dictionary<Items, int> m_Inventory;

    public void Initialize()
    {
        GameManager.Instance.Saving.RegisterSaveable(SaveManager.SaveOrder.Player, this);

        m_Team = new TeamMember[] { new TeamMember(), new TeamMember(), new TeamMember(), new TeamMember()};
        m_Inventory = new Dictionary<Items, int>();
    }

    public string[] GetSaveableData()
    {
        List<string> result = new List<string>();

        result.Add(m_Money.ToString());
        result.Add(m_JamCount.ToString());
        result.Add(m_JamWins.ToString());

        result.AddRange(m_Team[0].GetData());
        result.AddRange(m_Team[1].GetData());
        result.AddRange(m_Team[2].GetData());
        result.AddRange(m_Team[3].GetData());

        for(int i = 0; i < (int)Items.Count; i++)
        {
            result.Add(m_Inventory[(Items)i].ToString());
        }

        return result.ToArray();
    }

    public void SetSaveableData()
    {
        m_Money = 0;
        m_JamCount = 0;
        m_JamWins = 0;

        m_Team[0].SetData();
        m_Team[0].Programming += 2;
        m_Team[0].Debugging += 2;
        m_Team[0].Creativity += 1;

        m_Team[1].SetData();
        m_Team[1].Art += 3;
        m_Team[1].Creativity += 2;

        m_Team[2].SetData();
        m_Team[2].Audio += 3;
        m_Team[2].Creativity += 1;
        m_Team[2].Debugging += 1;

        m_Team[3].SetData();
        m_Team[3].Programming += 1;
        m_Team[3].Debugging += 1;
        m_Team[3].Creativity += 1;
        m_Team[3].Art += 1;
        m_Team[3].Audio += 1;

        for (int i = 0; i < (int)Items.Count; i++)
        {
            m_Inventory[(Items)i] = 0;
        }
    }

    public void SetSaveableData(string[] data)
    {
        m_Money = int.Parse(data[0]);
        m_JamCount = int.Parse(data[1]);
        m_JamWins = int.Parse(data[2]);

        int index = m_Team[0].SetData(data, 3);
        index = m_Team[1].SetData(data, index);
        index = m_Team[2].SetData(data, index);
        index = m_Team[3].SetData(data, index);
        
        for (int i = 0; i < (int)Items.Count; i++)
        {
            m_Inventory[(Items)i] = int.Parse(data[index + i]);
        }
    }

    public TeamMember[] GetTeam()
    {
        return m_Team;
    }
}
