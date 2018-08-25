using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public GameObject DayNightObject;
    public float RotationSpeed = 2.0f;

    private bool m_Start = false;

	public void Initialize ()
    {
        m_Start = true;
	}
	
	void Update ()
    {
        if (m_Start)
        {
            DayNightObject.transform.Rotate(new Vector3(RotationSpeed * Time.deltaTime, 0.0f, 0.0f));
        }
	}
}
