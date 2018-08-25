using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeight : MonoBehaviour
{
    public float OffsetDeadzone = 4.0f;
    public Shader PostProcess;

    private GameObject m_Player;
    private float m_Percentage = 0.0f;

    private Material m_PostProcess;

    void Start ()
    {
        m_Player = GameManager.Instance.GetPlayer();

        m_PostProcess = new Material(PostProcess);
    }
	
	void Update ()
    {
		if (m_Player.transform.position.y >= transform.position.y + OffsetDeadzone)
        {
            Vector3 position = transform.position;
            position.y = m_Player.transform.position.y - OffsetDeadzone;
            transform.position = position;
        }

        if (m_Player.transform.position.y <= transform.position.y - OffsetDeadzone)
        {
            Vector3 position = transform.position;
            position.y = m_Player.transform.position.y + OffsetDeadzone;
            transform.position = position;
        }
	}

    public void SetPercentage(float percentage)
    {
        m_Percentage = percentage;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_PostProcess.SetFloat("_PercentDark", m_Percentage);
        Graphics.Blit(source, destination, m_PostProcess);
        //Camera.main.targetTexture = source;
    }
}
