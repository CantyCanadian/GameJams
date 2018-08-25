using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]
public class Tree : MonoBehaviour {
    GameObject ThePlayer;
    public Sprite[] Sprites;
    public float MyTime = 0.0f;
    public bool PlayerHoldingMe = false;
    SpriteRenderer Renderer;
    BoxCollider2D MyCollider;
	// Use this for initialization
	void Start () {
        Renderer = GetComponent<SpriteRenderer>();
        MyCollider = GetComponent<BoxCollider2D>();
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        TimeChanged();
	}
	
	// Update is called once per frame
	void Update () {
        if(PlayerHoldingMe)
        {
            transform.position = ThePlayer.transform.position;
        }
	}

    void TimeChanged()
    {
        Renderer.sprite = Sprites[(int)MyTime];
        MyCollider.size = new Vector2(Renderer.sprite.bounds.size.x / 2, Renderer.sprite.bounds.size.y / 1.5f);
        MyCollider.offset = new Vector2(MyCollider.offset.x, 0.5f * (MyTime + 1));
    }

    void IncrementTime()
    {
        if (!PlayerHoldingMe)
        {
            if (MyTime < 3)
            {
                MyTime++;
            }
            else MyTime = 0;
            TimeChanged();
        }
    }

    void DecrementTime()
    {
        if (!PlayerHoldingMe)
        {
            if (MyTime > 0)
            {
                MyTime--;
            }
            else MyTime = 3;
            TimeChanged();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            if (MyTime != 0.0f && transform.position.z == 0)
                other.transform.gameObject.SendMessage("SetCanClimb", true);
            else
            {
                if(PlayerHoldingMe == false)
                {
                    other.transform.gameObject.SendMessage("SetCantClimb", false);
                }

                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerHoldingMe)
                    {
                        PlayerHoldingMe = false;
                        SpriteRenderer renderer = ThePlayer.GetComponent<SpriteRenderer>();
                        transform.position = new Vector3(transform.position.x, (transform.position.y - renderer.bounds.size.y / 2 + 0.05f));
                        Renderer.sortingOrder = -1;
                    }
                    else if(transform.position.z == 0)
                    {
                        PlayerHoldingMe = true;
                        Renderer.sortingOrder = 1;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            other.transform.gameObject.SendMessage("SetCantClimb", false);
        }
    }
}
