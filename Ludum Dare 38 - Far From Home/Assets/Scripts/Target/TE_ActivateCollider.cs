using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TE_ActivateCollider : MonoBehaviour, ITargetEffect
{
    private Collider m_Collider;

    void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Collider.enabled = false;
    }

    public void Activate()
    {
        m_Collider.enabled = true;
    }
}
