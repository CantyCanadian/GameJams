using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugVelocity : MonoBehaviour
{
    private GameObject m_Player;
    private Text m_Text;

	void Start ()
    {
        m_Text = GetComponent<Text>();
	}
	
	void Update ()
    {
		if (m_Player != null)
        {
            Vector3 velocity = m_Player.GetComponent<Movement>().GetVelocity();
            m_Text.text = "Velocity : \nx - " + velocity.x + "\ny - " + velocity.y + "\nz - " + velocity.z;
        }
        else
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
}
