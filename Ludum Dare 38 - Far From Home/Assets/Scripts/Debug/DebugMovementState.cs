using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMovementState : MonoBehaviour
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
            m_Text.text = "Movement State : " + m_Player.GetComponent<Movement>().GetMovementState();
        }
        else
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
}
