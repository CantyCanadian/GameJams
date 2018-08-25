using UnityEngine;
using System.Collections;

public class MountainTeleport : MonoBehaviour {

    GameObject player;
    public GameObject target;

    public Vector3 offset;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        player.transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, player.transform.position.z + offset.z);
        player.SendMessage("SetToZero");
    }
}
