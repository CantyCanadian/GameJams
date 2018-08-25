using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCanvas : MonoBehaviour
{
    public GameObject CanvasPivot;
    public GameObject Bar;
    public BombBoi Base;
    	
	void Update ()
    {
        Vector3 rotation = CanvasPivot.transform.eulerAngles;
        CanvasPivot.transform.LookAt(Camera.main.transform.position);
        CanvasPivot.transform.eulerAngles = new Vector3(rotation.x, CanvasPivot.transform.eulerAngles.y, rotation.z);

        Bar.GetComponent<RectTransform>().sizeDelta = new Vector2(1.0f - Base.GetTimer(), 0.15f);
        Bar.GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, Base.GetTimer());
    }
}
