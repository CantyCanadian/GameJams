using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugAbility : MonoBehaviour
{
    private GameObject m_Player;
    private Text m_Text;

    void Start()
    {
        m_Text = GetComponent<Text>();
    }

    void Update()
    {
        if (m_Player != null)
        {
            var ability = m_Player.GetComponent<Abilities>().GetAbilityData();
            m_Text.text = "Current Ability : " + ability.Key + "\nAbility Index : " + ability.Value;
        }
        else
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
}
