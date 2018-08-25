using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouda : MonoBehaviour {

    public float CloudSpeed = 10.0f;

    private Material m_Mat;
    private float m_Matoffset = 0.0f;

    private void Start()
    {
        m_Mat = GetComponent<Material>();
    }

    void Update ()
    {
        m_Matoffset += Time.deltaTime * CloudSpeed;
        m_Mat.SetTextureOffset("_MainTex", new Vector2(0.0f, m_Matoffset));
	}
}
