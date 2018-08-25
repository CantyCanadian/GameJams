using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMainMenu : MonoBehaviour {

    public float RotationPerSecond = 45.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update ()
    {
        transform.Rotate(new Vector3(0.0f, RotationPerSecond * Time.deltaTime, 0.0f));
	}
}
