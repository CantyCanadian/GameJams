using UnityEngine;
using System.Collections;

public class WindMillSpinner : MonoBehaviour {
    float MyTime = 0.0f;
    public Sprite[] Sprites;
    SpriteRenderer Renderer;
	// Use this for initialization
	void Start () {
        Renderer = GetComponent<SpriteRenderer>();
        TimeChanged();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * -1.0f);
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
    }
}
