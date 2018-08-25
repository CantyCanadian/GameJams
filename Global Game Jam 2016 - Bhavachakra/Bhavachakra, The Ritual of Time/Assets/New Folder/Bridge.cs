using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour {
    float MyTime = 0.0f;
    BoxCollider2D MyCollider;
    public Sprite[] Sprites;
    SpriteRenderer Renderer;

    public GameObject Plank;

    bool canBuild = false;

	// Use this for initialization
	void Start () {
        MyCollider = GetComponent<BoxCollider2D>();
        MyCollider.enabled = false;
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = Sprites[(int)MyTime];
        TimeChanged();
	}
	
	// Update is called once per frame
	void Update () {

	}
    void IncrementTime()
    {
        if (canBuild)
        {
            if (MyTime < 3)
            {
                MyTime++;
                TimeChanged();
            }
        }
    }

    void DecrementTime()
    {
        if (canBuild)
        {
            if (MyTime > 0)
            {
                MyTime--;
                TimeChanged();
            }
        }
    }

    void TimeChanged()
    {
        if (canBuild == true)
        {
            MyCollider.enabled = true;
            Renderer.enabled = true;
            Plank.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Plank.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Renderer.sprite = Sprites[(int)MyTime];
            MyCollider.size = new Vector2(Renderer.sprite.bounds.size.x, MyCollider.size.y);
            MyCollider.offset = new Vector2(0.3f * (MyTime + 1), MyCollider.offset.y);
        }
    }

    void SetCanBuild()
    {
        Debug.Log("I can be built now, sir.");
        canBuild = true;
    }
}
