using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRotation : MonoBehaviour
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
            Vector3 rotation = m_Player.GetComponent<Movement>().GetRotationData();
            m_Text.text = "Rotation : \nAngle - " + rotation.x + "\nPercentage - " + rotation.y + "\nIs Done Changing - " + (rotation.z == 1.0f ? "true" : "false");
        }
        else
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
}
