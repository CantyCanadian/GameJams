using UnityEngine;
using System.Collections;

public class DetectPlank : MonoBehaviour {

    public GameObject bridge;
    public GameObject plank;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Plank")
        {
            Debug.Log("I am indeed colliding, sir.");
            bridge.gameObject.SendMessage("SetCanBuild");
        }
    }
}
