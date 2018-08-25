using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugJump : MonoBehaviour
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
            Vector2 jump = m_Player.GetComponent<Movement>().GetJumpingData();
            m_Text.text = "Jump : \nPercentage - " + jump.x + "\nIs Jumping - " + (jump.y == 1.0f ? "true" : "false");
        }
        else
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
}
