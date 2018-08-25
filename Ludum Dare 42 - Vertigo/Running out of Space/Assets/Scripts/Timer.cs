using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimerText;
    private float m_Timer = 0.0f;

    void Update ()
    {
        m_Timer += Time.deltaTime;

        TimerText.text = Mathf.Floor(m_Timer * 100.0f).ToString();
    }
}
