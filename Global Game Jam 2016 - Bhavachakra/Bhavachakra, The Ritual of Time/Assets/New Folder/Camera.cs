using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
    GameObject ThePlayer;
	// Use this for initialization
	void Start () {
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(ThePlayer.transform.position.x, ThePlayer.transform.position.y, transform.position.z);
	}
}
