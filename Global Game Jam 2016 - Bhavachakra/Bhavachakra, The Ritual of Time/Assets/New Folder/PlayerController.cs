using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float MoveSpeed = 3.0f;
    public float JumpSpeed = 3.0f;
    public float MyTime = 0.0f;
    //float FallSpeed = 5.0f;
    Vector3 ForwardDirection;
    BoxCollider2D MyCollider;
    //CharacterController MyController;
    public GameObject[] AllObjects;
    public bool CanClimb = false;
    public bool IsHoldingBalloon = false;
    public GameObject Balloon;
    float WalkCycleRotation = 0.0f;
    int WalkCycleDirection = 1;

     bool isJumping = false;
     //float baseHeight = 0.0f;
     float verticalNew;

    

     SpriteRenderer Renderer;
     public Sprite[] Sprites;
     public GameObject camera;

     bool GameOver = false;
     public GameObject WindMill;

    public Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
	    //MyController = GetComponent<CharacterController>();
        MyCollider = GetComponent<BoxCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();
        AllObjects = GameObject.FindGameObjectsWithTag("TimeChanger");
        rb2d = GetComponent<Rigidbody2D>();
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        MyCollider.size = new Vector2(Renderer.sprite.bounds.size.x / 2, Renderer.sprite.bounds.size.y);
        TimeChanged();
	}
	void Update()
    {
        if (GameOver == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (MyTime < 3)
                {
                    MyTime++;
                    for (int i = 0; i < AllObjects.Length; i++)
                    {
                        AllObjects[i].SendMessage("IncrementTime");
                    }
                    TimeChanged();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (MyTime > 0)
                {
                    MyTime--;
                    for (int i = 0; i < AllObjects.Length; i++)
                    {
                        AllObjects[i].SendMessage("DecrementTime");
                    }
                    TimeChanged();
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                WalkCycleDirection = 1;
                WalkCycleRotation = 0.0f;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                WalkCycleDirection = 1;
                WalkCycleRotation = 0.0f;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        else
        {
            if(transform.position.y < WindMill.gameObject.transform.position.y + 10.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime);
                GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
	// Update is called once per frame
    void FixedUpdate()
    {
        if (GameOver == false)
        {
            float horizontal = Input.GetAxis("Horizontal");

            float vertical = Input.GetAxis("Vertical");
            if (!CanClimb || MyTime == 3 || MyTime == 0)
                vertical = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isJumping == false && MyTime != 3)
                {
                    vertical += JumpSpeed;

                    isJumping = true;
                }
            }
                
            ForwardDirection = new Vector2(horizontal, vertical);

            rb2d.MovePosition(transform.position + (ForwardDirection * MoveSpeed * Time.deltaTime));
        }
    }

    void TimeChanged()
    {
        Renderer.sprite = Sprites[(int)MyTime];

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -4 + (MyTime * -1.5f));

        JumpSpeed = 8 + (MyTime * 3);
        MoveSpeed = 10 - (MyTime * 1.5f);
        
        MyCollider.size = new Vector2(Renderer.sprite.bounds.size.x, Renderer.sprite.bounds.size.y);

        if (MyTime == 0)
        {
            Balloon.SendMessage("CanPickUp");
        }
        else Balloon.SendMessage("CantPickUp");
    }
    void SetCanClimb()
    {
        CanClimb = true;
    }

    void SetCantClimb()
    {
        CanClimb = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor")
        {
            isJumping = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == WindMill)
        {
            if(MyTime == 3 && IsHoldingBalloon)
            {
                GameOver = true;
            }
        }
    }

    void HasBalloon()
    {
        if (MyTime == 0)
        {
            IsHoldingBalloon = true;
            Balloon.SendMessage("PlayerCanPickUp");
        }

    }

    void SetToZero()
    {
        MyTime = 0;
        TimeChanged();
    }
}
