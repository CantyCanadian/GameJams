using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{


	void Start ()
    {
		
	}
	
	void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Mouse")))
        {
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;
            newPos.z = hit.point.z;
            transform.position = newPos;
        }
	}
}
