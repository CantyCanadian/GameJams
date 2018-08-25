using UnityEngine;
using System.Collections;

public class WoodPlank : MonoBehaviour
{
    public bool PlayerHoldingMe = false;
    GameObject ThePlayer;
    // Use this for initialization
    void Start()
    {
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHoldingMe)
        {
            transform.position = new Vector3(ThePlayer.transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2, ThePlayer.transform.position.y);
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerHoldingMe)
                {
                    PlayerHoldingMe = false;
                    SpriteRenderer renderer = ThePlayer.GetComponent<SpriteRenderer>();
                    transform.position = new Vector3(transform.position.x, (transform.position.y - renderer.bounds.size.y / 2 + 0.5f));
                }
                else
                {
                    PlayerHoldingMe = true;
                }
            }

        }
    }
}
