using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotContainer : MonoBehaviour
{
    public GameObject Resizeable;
    public Vector2 SizeOn;
    public Vector2 SizeOff;

    private bool m_Flag = false;
    private RectTransform m_RectTransform = null;

    public void SetFlag(bool flag)
    {
        if (m_RectTransform == null)
        {
            m_RectTransform = Resizeable.GetComponent<RectTransform>();
        }

        m_Flag = flag;

        m_RectTransform.sizeDelta = (m_Flag ? SizeOn : SizeOff);
    }
}
