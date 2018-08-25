using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public float JumpStrength;
    public float JumpLockTimer;

    private Rigidbody m_Rigidbody;
    private PlayerState m_PlayerState = PlayerState.Null;

    private bool m_JumpLock = false;

    public void Start()
    {
        StartCoroutine(PlayerStateLoop());
    }

    public void SetStun(bool flag)
    {
        if (flag)
        {
            m_PlayerState = PlayerState.Stun;
        }
        else
        {
            m_PlayerState = PlayerState.Null;
        }
    }

    public IEnumerator PlayerStateLoop()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_PlayerState == PlayerState.Null)
            {
                m_PlayerState = PlayerState.JumpUp;
            }

            switch (m_PlayerState)
            {
                case PlayerState.JumpUp:
                    yield return StartCoroutine(JumpLoop());
                    break;

                default:
                    break;
            }

            yield return null;
        }
    }

    private IEnumerator JumpLoop()
    {
        float delta = 0.0f;
        float tenth = 0.0f;
        while(true)
        {
            if (m_PlayerState == PlayerState.JumpUp)
            {
                delta += Time.deltaTime;
                tenth -= Time.deltaTime;

                if (Input.GetKey(KeyCode.Space) && delta <= JumpLockTimer)
                {
                    if (tenth <= 0.0f)
                    {
                        m_Rigidbody.AddForce(new Vector3(0.0f, JumpStrength, 0.0f), ForceMode.VelocityChange);
                        tenth = 0.1f;
                    }
                }
                else
                {
                    m_PlayerState = PlayerState.JumpDown;
                    delta = 0.0f;
                }
            }
            else if (m_PlayerState == PlayerState.JumpDown)
            {
                m_JumpLock = true;
                yield return new WaitUntil(() => m_JumpLock == false);
                m_PlayerState = PlayerState.Null;
            }
            else
            {
                break;
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            m_JumpLock = false;
        }
    }

    private enum PlayerState
    {
        JumpUp,
        JumpDown,
        Stun,
        Null
    }
}
