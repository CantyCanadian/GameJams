using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TE_ActivateGravity : MonoBehaviour, ITargetEffect
{
    public float FreezeObjectAfter = 0.5f;
    private Rigidbody m_RigidBody;

    void Start ()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.useGravity = false;
	}

    public void Activate()
    {
        m_RigidBody.useGravity = true;

        Invoke("FreezeObject", FreezeObjectAfter);
    }

    void FreezeObject()
    {
        GetComponent<Collider>().enabled = false;
        m_RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        m_RigidBody.useGravity = false;
    }
}
