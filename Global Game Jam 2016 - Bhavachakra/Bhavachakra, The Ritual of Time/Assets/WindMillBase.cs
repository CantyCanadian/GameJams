using UnityEngine;
using System.Collections;

public class WindMillBase : MonoBehaviour {
    float MyTime = 0.0f;
    public Sprite[] Sprites;
    SpriteRenderer Renderer;
    public GameObject Spinner;
    BoxCollider2D collider;

    Vector3 StartPos;
	// Use this for initialization
	void Start () {
        Renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        StartPos = transform.position;
        TimeChanged();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void IncrementTime()
    {
        if (MyTime < 1)
            MyTime++;

        TimeChanged();
    }

    void DecrementTime()
    {
        if (MyTime > 0)
            MyTime--;
        TimeChanged();
    }

    void TimeChanged()
    {
        Renderer.sprite = Sprites[(int)MyTime];
        Spinner.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y + Renderer.sprite.bounds.size.y / 2 - 0.3f);
        transform.position = new Vector3(transform.position.x, StartPos.y + (3.0f * MyTime));
        collider.size = Renderer.sprite.bounds.size;
    }
}
