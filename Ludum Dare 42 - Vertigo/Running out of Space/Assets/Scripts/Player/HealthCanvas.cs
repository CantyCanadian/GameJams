using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    public GameObject CanvasPivot;
    public GameObject[] FullHearts;
    public BaseEnemy Base;

    private int m_LastHealth = -1;
    	
	void Update ()
    {
        Vector3 rotation = CanvasPivot.transform.eulerAngles;
        CanvasPivot.transform.LookAt(Camera.main.transform.position);
        CanvasPivot.transform.eulerAngles = new Vector3(rotation.x, CanvasPivot.transform.eulerAngles.y, rotation.z);

        int health = Base.GetHealth();

        if (health != m_LastHealth)
        {
            for(int i = 0; i < FullHearts.Length; i++)
            {
                FullHearts[i].SetActive(false);
            }

            for (int i = 0; i < health; i++)
            {
                FullHearts[i].SetActive(true);
            }
        }
    }
}
