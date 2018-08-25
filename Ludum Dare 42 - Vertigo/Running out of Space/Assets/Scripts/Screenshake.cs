using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : Singleton<Screenshake>
{
    public GameObject Player;

    public float Strength = 0.3f;
    private Vector3 m_Offset;
    private float m_Time = -2.0f;

    private void Start()
    {
        m_Offset = transform.localPosition;
        StartCoroutine(ShakeLoop());
    }

    public void Shake(float time)
    {
        m_Time = Mathf.Max(m_Time, time);
    }

    private IEnumerator ShakeLoop()
    {
        while(true)
        {
            if (m_Time > 0.0f)
            {
                Vector3 shake = Random.insideUnitSphere * Strength;
                transform.localPosition = m_Offset + shake;
                m_Time -= Time.deltaTime;
            }
            else if (m_Time != -2.0f)
            {
                m_Time = -2.0f;
                transform.localPosition = m_Offset;
            }

            yield return null;
        }
    }
}
