using UnityEngine;
using System.Collections;

public class Balloons : MonoBehaviour {
    bool CanPlayerGrabMe = false;
    bool IsPlayerHoldingMe = false;
    GameObject ThePlayer;
    SpriteRenderer Renderer;
	// Use this for initialization
	void Start () {
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        Renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(IsPlayerHoldingMe)
        {
            transform.position = new Vector3(ThePlayer.transform.position.x, ThePlayer.transform.position.y + 0.7f);
        }
	}

    void CanPickUp()
    {
        if (!IsPlayerHoldingMe)
        {
            CanPlayerGrabMe = true;
            Renderer.enabled = true;
        }
    }

    void CantPickUp()
    {
        if (!IsPlayerHoldingMe)
        {
            CanPlayerGrabMe = false;
            Renderer.enabled = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == ThePlayer)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ThePlayer.SendMessage("HasBalloon");
            }
        }
    }

    void PlayerCanPickUp()
    {
        IsPlayerHoldingMe = true;
    }
}
