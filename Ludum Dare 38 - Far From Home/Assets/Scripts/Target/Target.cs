using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject[] TargetEffect;
    public bool DestroyOnActivate = false;

    private List<ITargetEffect> m_TargetEffect;

    void Start()
    {
        m_TargetEffect = new List<ITargetEffect>();

        foreach(GameObject go in TargetEffect)
        {
            if (go != null)
            {
                ITargetEffect ite = go.GetComponent<ITargetEffect>();
                if (ite != null)
                {
                    m_TargetEffect.Add(ite);
                }
            }
        }
    }

    public void Activate()
    {
        foreach (ITargetEffect ite in m_TargetEffect)
        {
            ite.Activate();
        }

        if (DestroyOnActivate)
        {
            Destroy(gameObject);
        }
    }
}
