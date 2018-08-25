using UnityEngine;
using System.Collections;

public class Cinematics : MonoBehaviour {

    public GameObject player;
    public GameObject balloon;
	public GameObject screen;
    public GameObject screen2;

    int set = 0;

    Vector3 startingPos;

    bool isEnabled = false;

	// Use this for initialization
	void Start () {

        balloon.transform.position = player.transform.position;
        startingPos = balloon.transform.position;

        //isEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return) && set == 1)
        {
            screen2.GetComponent<SpriteRenderer>().enabled = false;
            isEnabled = true;
        }

		if(Input.GetKeyDown(KeyCode.Return) && set == 0)
		   {
			screen.GetComponent<SpriteRenderer>().enabled = false;
            screen2.GetComponent<SpriteRenderer>().enabled = true;
            set++;
		}

        if (isEnabled == true)
        {
            balloon.transform.Translate(Vector2.up * Time.deltaTime * 2.0f);

            if (balloon.transform.position.y >= startingPos.y + 10.0f)
            {
                isEnabled = false;
            }
        }

        if(enabled == false)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}
