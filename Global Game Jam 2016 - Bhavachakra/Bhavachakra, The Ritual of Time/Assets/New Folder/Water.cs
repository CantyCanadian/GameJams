using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public float MyTime = 0.0f;
    BoxCollider2D MyCollider;
    public Sprite[] Sprites;
    public bool CanKillPlayer = false;
    SpriteRenderer Renderer;

    private Vector3 SetPosition;

    private bool dirRight = true;
    public float speed = 2.0f;
	// Use this for initialization
	void Start () {
        MyCollider = GetComponent<BoxCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();
        TimeChanged();
        SetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (dirRight)
            transform.Translate(Vector2.right * (speed * ((SetPosition.x + 1.1f) - transform.position.x)) * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * (speed * (transform.position.x-(SetPosition.x - 1.1f))) * Time.deltaTime);


        if (transform.position.x >= SetPosition.x + 1.0f)
        {
            dirRight = false;
        }

        if (transform.position.x <= SetPosition.x - 1)
        {
            dirRight = true;
        }

    }


    void IncrementTime()
    {
        if (MyTime < 2)
        {
            MyTime++;
            TimeChanged();
        }
    }

    void DecrementTime()
    {
        if (MyTime > 0)
        {
            MyTime--;
            TimeChanged();
        }
    }

    void TimeChanged()
    {
        switch((int)MyTime)
        {
            case 0:
                Renderer.enabled = true;
                Renderer.sprite = Sprites[(int)MyTime];
                MyCollider.enabled = true;
                CanKillPlayer = true;
                break;
            case 1:
                Renderer.enabled = true;
                Renderer.sprite = Sprites[(int)MyTime];
                MyCollider.enabled = true;
                CanKillPlayer = true;             
                break;
            case 2:
                Renderer.enabled = false;
                MyCollider.enabled = false;
                CanKillPlayer = false;
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (CanKillPlayer == true)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
