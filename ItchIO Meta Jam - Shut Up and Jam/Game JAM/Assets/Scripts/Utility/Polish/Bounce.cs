﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Vector2 BounceDelta;
    public float BounceTime;

    private RectTransform m_Transform;
    private Vector2 m_OriginalPosition;

    private Coroutine m_CoroutineRef;

    public void Initialize()
    {
        m_Transform = GetComponent<RectTransform>();
    }

    public void Play()
    {
        if (m_CoroutineRef != null)
        { 
            Stop();
        }

        m_OriginalPosition = m_Transform.anchoredPosition;
        m_CoroutineRef = StartCoroutine(PlayLoop());
    }

    public void Stop()
    {
        if (m_CoroutineRef == null)
        {
            return;
        }

        StopCoroutine(m_CoroutineRef);
        m_CoroutineRef = null;
        m_Transform.anchoredPosition = m_OriginalPosition;
    }

    private IEnumerator PlayLoop()
    {
        float delta = 0.0f;

        while(true)
        {
            delta += Time.deltaTime;

            m_Transform.anchoredPosition = m_OriginalPosition + (BounceDelta * AbsSinOfPiX(delta));

            yield return null;
        }
    }

    private float AbsSinOfPiX(float x)
    {
        return Mathf.Abs(Mathf.Sin(Mathf.PI * x));
    }
}
