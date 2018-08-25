using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform RotatorOutside;
    public Transform RotatorInside;
    public float TimeToRotate = 1.0f;
    public float TimeToMove = 1.0f;

    public Camera MainCamera;
    public Camera AnimationCamera;
    public GameObject AnimatedObject;
    public GameObject Flower;
    public GameObject AcquiredText;

    public GameObject FinalAnimatedObject;
    public Camera FinalAnimationCamera;

    static public GameManager Instance
    {
        get { return m_Instance; }
    }

    static private GameManager m_Instance = null;

    private GameObject m_Player = null;
    private Vector3 m_RespawnPointPos;
    private Vector3 m_RespawnPointRot;
    private Vector3 m_RotatorRotation;
    private bool m_IsInside = false;
    private List<GameObject> m_ResetObjects = null;

    private bool m_IsRotating = false;
    private float m_OriginalRotation = 0.0f;
    private float m_RotationAngle = 0.0f;
    private float m_RotationDelta = 0.0f;
    private Transform m_RotationTransform;

    private bool m_MoveToInside = false;
    private bool m_IsMoving = false;
    private bool m_HasMoved = false;
    private float m_MoveDelta = 0.0f;
    private float m_Percentage = 0.0f;

    void Start()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_ResetObjects = new List<GameObject>();

        RotatorInside.gameObject.SetActive(false);
        RotatorOutside.gameObject.SetActive(true);

        AnimatedObject.GetComponent<Animator>().SetBool("IsPlaying", false);
    }

    void Update()
    {
        if (m_IsRotating)
        {
            m_RotationDelta += Time.deltaTime;

            RotatorOutside.transform.eulerAngles = Vector3.Lerp(new Vector3(0.0f, m_OriginalRotation, 0.0f), new Vector3(0.0f, m_OriginalRotation + m_RotationAngle, 0.0f), m_RotationDelta / TimeToRotate);
            RotatorInside.transform.eulerAngles = RotatorOutside.transform.eulerAngles;

            if (m_RotationDelta >= TimeToRotate)
            {
                m_IsRotating = false;
                FinalizeRotation();
            }
        }

        if (m_IsMoving)
        {
            m_MoveDelta += Time.deltaTime;

            if (m_HasMoved == false && m_MoveDelta >= TimeToMove / 2.0f)
            {
                if (m_MoveToInside)
                {
                    RotatorInside.gameObject.SetActive(true);
                    RotatorOutside.gameObject.SetActive(false);
                }
                else
                {
                    RotatorInside.gameObject.SetActive(false);
                    RotatorOutside.gameObject.SetActive(true);
                }

                m_HasMoved = true;
            }

            if (m_MoveDelta < TimeToMove / 2.0f)
            {
                m_Percentage = m_MoveDelta / (TimeToMove / 2.0f);
            }
            else
            {
                m_Percentage = 1.0f - (m_MoveDelta - (TimeToMove / 2.0f)) / (TimeToMove / 2.0f);
            }

            if (m_MoveDelta >= TimeToMove)
            {
                m_IsMoving = false;
                m_Player.GetComponent<Movement>().SetLockedMovement(false);
            }

            Camera.main.GetComponent<CameraHeight>().SetPercentage(m_Percentage);
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        Debug.Log("Registered Player");
        m_Player = player;
    }

    public void RegisterResetObject(GameObject resetObject)
    {
        foreach(GameObject go in m_ResetObjects)
        {
            if (go.GetInstanceID() == resetObject.GetInstanceID())
            {
                return;
            }
        }

        m_ResetObjects.Add(resetObject);
    }

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public void SetRespawnPoint(Transform point,float rotator, bool isInside)
    {
        m_RespawnPointPos = point.transform.position;
        m_RespawnPointRot = point.transform.eulerAngles;
        m_RotatorRotation = new Vector3(0.0f,rotator,0.0f);
        m_IsInside = isInside;
    }

    public void TeleportPlayer(GameObject point, float rotatorRotation, bool isInside)
    {
        m_Player.transform.position = point.transform.position;
        m_Player.transform.eulerAngles = point.transform.eulerAngles;

        RotatorOutside.transform.eulerAngles = new Vector3(0.0f,rotatorRotation,0.0f);
        RotatorInside.transform.eulerAngles = RotatorOutside.transform.eulerAngles;

        if(isInside)
        {
            RotatorInside.gameObject.SetActive(true);
            RotatorOutside.gameObject.SetActive(false);
        }
        else
        {
            RotatorInside.gameObject.SetActive(false);
            RotatorOutside.gameObject.SetActive(true);
        }

        foreach (GameObject go in m_ResetObjects)
        {
            go.SendMessage("Reset");
        }
    }

    public void RespawnPlayer()
    {
        m_Player.transform.position = m_RespawnPointPos;
        m_Player.transform.eulerAngles = m_RespawnPointRot;

        RotatorOutside.transform.eulerAngles = m_RotatorRotation;
        RotatorInside.transform.eulerAngles = m_RotatorRotation;

        if (m_IsInside)
        {
            RotatorInside.gameObject.SetActive(true);
            RotatorOutside.gameObject.SetActive(false);
        }
        else
        {
            RotatorInside.gameObject.SetActive(false);
            RotatorOutside.gameObject.SetActive(true);
        }

        foreach (GameObject go in m_ResetObjects)
        {
            go.SendMessage("Reset");
        }
    }

    public void InitializeEntry(bool moveToInside)
    {
        m_MoveDelta = 0.0f;
        m_IsMoving = true;
        m_HasMoved = false;
        m_MoveToInside = moveToInside;
        m_Player.GetComponent<Movement>().SetLockedMovement(true);
    }

    public void InitiateRotation(float angle, Transform rotationTransform, bool isInside)
    {
        if (isInside)
        {
            m_Player.transform.parent = RotatorInside.transform;
        }
        else
        {
            m_Player.transform.parent = RotatorOutside.transform;
        }
        m_RotationTransform = rotationTransform;
        m_RotationAngle = angle;
        m_OriginalRotation = RotatorOutside.transform.eulerAngles.y;
        m_IsRotating = true;
        m_RotationDelta = 0.0f;
        m_Player.GetComponent<Movement>().SetLockedMovement(true);
    }

    private void FinalizeRotation()
    {
        m_Player.transform.parent = null;

        m_Player.transform.position = m_RotationTransform.transform.position;
        m_Player.transform.Rotate(new Vector3(0.0f, -m_RotationAngle, 0.0f));
        m_Player.GetComponent<Movement>().SetLockedMovement(false);
    }

    public void WatchAnimationHover()
    {
        AnimatedObject.GetComponent<Animator>().enabled = true;
        MainCamera.GetComponent<CameraHeight>().enabled = false;
        Vector3 position = MainCamera.transform.position;
        MainCamera.transform.position = AnimationCamera.transform.position;

        AnimationCamera.transform.position = position;

        AnimatedObject.GetComponent<Animator>().SetBool("IsPlaying", true);

        m_Player.GetComponent<Movement>().SetLockedMovement(true);
        Invoke("FinishAnimationHover", 8.5f);
    }

    private void FinishAnimationHover()
    {
        MainCamera.GetComponent<CameraHeight>().enabled = true;
        Vector3 position = MainCamera.transform.position;
        MainCamera.transform.position = AnimationCamera.transform.position;

        AnimationCamera.transform.position = position;

        AnimatedObject.GetComponent<Animator>().SetBool("IsPlaying", false);

        m_Player.GetComponent<Movement>().SetLockedMovement(false);
        m_Player.GetComponent<Abilities>().EnableAbility(Abilities.PossibleAbilities.Hover);
        Destroy(Flower);

        AcquiredText.SetActive(true);

        Invoke("RemoveText", 3.0f);
    }

    private void RemoveText()
    {
        AcquiredText.SetActive(false);
    }

    public void PlayFinalAnimation()
    {
        SceneManager.LoadScene("MainMenu");
        //FinalAnimatedObject.GetComponent<Animator>().enabled = true;
        //MainCamera.GetComponent<CameraHeight>().enabled = false;
        //Vector3 position = MainCamera.transform.position;
        //MainCamera.transform.position = FinalAnimationCamera.transform.position;

        //FinalAnimationCamera.transform.position = position;

        ////AnimatedObject.GetComponent<Animator>().SetBool("IsPlaying", true);

        //m_Player.GetComponent<Movement>().SetLockedMovement(true);
        //Invoke("FinishFinalAnimation", 4.2f);
    }

    private void FinishFinalAnimation()
    {
    }
}
