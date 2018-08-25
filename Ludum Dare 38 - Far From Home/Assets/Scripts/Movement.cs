using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private enum MovementState
    {
        OnGround,
        InAir,
        Hovering
    }

    public GameObject PlayerModel;
    public GameObject FootObject;
    public GameObject HeadObject;
    public GameObject LeftObject;
    public GameObject RightObject;

    public float GravityStrength = -9.8f;
    public float HoveringGravityStrength = -2.0f;
    public float VerticalEpsilon = 0.005f;

    public float MovementSpeed = 4.0f;
    public float MovementEpsilon = 0.005f;

    public float TimeToRotate = 0.5f;

    public float JumpStrength = 10.0f;
    public float TimeToJump = 1.0f;


    private MovementState m_MovementState = MovementState.InAir;
    private Vector3 m_Velocity = Vector3.zero;
    private Abilities m_Abilities;

    private float m_RotationBuffer = 1.0f;
    private float m_TargetRotation = 0.0f;
    private float m_OldRotation = 0.0f;
    private float m_RotationDelta = 0.0f;
    private bool m_HasReachedRotation = true;

    private float m_JumpDelta = 0.0f;
    private bool m_IsJumping = false;

    private bool m_LockMovement = false;

    void Start ()
    {
        GameManager.Instance.RegisterPlayer(gameObject);
        GameManager.Instance.SetRespawnPoint(transform, 180.0f, false);

        m_Abilities = GetComponent<Abilities>();
	}
	
	void Update ()
    {
        if (!m_LockMovement)
        {
            CalculateCollisions();
            CalculateGravity();
            CalculateMovement();
            CalculateRotation();
            CalculateJump();
            CalculateEpsilons();

            transform.position += m_Velocity * Time.deltaTime;
        }
	}

    void CalculateCollisions()
    {
        if (m_MovementState != MovementState.OnGround)
        {
            // Hitting your head on the ceiling will make you stop jumping.
            if (CollisionRaycast(HeadObject))
            {
                m_IsJumping = false;
            }

            // Hitting the floor will place your character right and stop gravity.
            RaycastHit footHit;
            if (CollisionRaycast(FootObject, out footHit))
            {
                ChangeState(MovementState.OnGround);
            }
        }
        else
        {
            // If you left the floor.
            RaycastHit footHit;
            if (!CollisionRaycast(FootObject, out footHit))
            {
                ChangeState(MovementState.InAir);
            }
        }

        RaycastHit leftHit;
        if (CollisionRaycast(LeftObject, out leftHit))
        {
            transform.position = leftHit.point + (Vector3.left * Vector3.Distance(LeftObject.transform.position, transform.position));
        }

        RaycastHit rightHit;
        if (CollisionRaycast(RightObject, out rightHit))
        {
            transform.position = rightHit.point + (Vector3.right * Vector3.Distance(RightObject.transform.position, transform.position));
        }
    }

    private bool CollisionRaycast(GameObject target)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit, ~LayerMask.GetMask("Player")))
        {
            if (hit.distance <= Vector3.Distance(transform.position, target.transform.position))
            {
                return true;
            }
        }

        return false;
    }

    private bool CollisionRaycast(GameObject target, out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit))
        {
            if (hit.distance <= Vector3.Distance(transform.position, target.transform.position))
            {
                return true;
            }
        }

        return false;
    }

    void CalculateGravity()
    {
        if(m_MovementState == MovementState.InAir)
        {
            m_Velocity.y += GravityStrength * Time.deltaTime;
        }
        
        if (m_MovementState == MovementState.Hovering)
        {
            m_Velocity.y = HoveringGravityStrength * Time.deltaTime;
        }
    }

    void CalculateMovement()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        float velocityDir = Mathf.Sign(m_Velocity.x);
        
        if (horizontalValue != 0.0f)
        {
            float yStorage = m_Velocity.y;
            m_Velocity = transform.forward * horizontalValue * MovementSpeed;
            m_Velocity.y = yStorage;
        }
        
        if (!Input.GetButton("Horizontal"))
        {
            m_Velocity.z = 0.0f;
        }
    }

    void CalculateRotation()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2.0f, Color.red);

        float horizontalValue = Input.GetAxis("Horizontal");

        if (horizontalValue != 0.0f && Mathf.Sign(horizontalValue) != m_RotationBuffer)
        {
            m_RotationBuffer = Mathf.Sign(horizontalValue);

            m_TargetRotation = m_RotationBuffer == 1.0f ? 0.0f : 180.0f;
            m_OldRotation = m_RotationBuffer == 1.0f ? 180.0f : 0.0f;

            m_RotationDelta = 0.0f;
            m_HasReachedRotation = false;
        }

        if (m_HasReachedRotation == false)
        {
            m_RotationDelta += Time.deltaTime;

            float rotation = Mathf.Lerp(m_OldRotation, m_TargetRotation, m_RotationDelta / TimeToRotate);
            PlayerModel.transform.eulerAngles = new Vector3(0.0f, rotation, 0.0f);

            if (m_RotationDelta >= TimeToRotate)
            {
                m_HasReachedRotation = true;
            }
        }
    }

    void CalculateJump()
    {
        if (m_IsJumping)
        {
            if (Input.GetButton("Jump"))
            {
                m_JumpDelta += Time.deltaTime;

                if (m_JumpDelta < TimeToJump)
                {
                    m_Velocity.y = JumpStrength;
                }
                else
                {
                    m_IsJumping = false;
                }
            }
            else
            {
                m_IsJumping = false;
            }
        }

        if (m_MovementState == MovementState.Hovering)
        {
            if (Input.GetButtonUp("Jump"))
            {
                ChangeState(MovementState.InAir);
            }
        }

        if (m_MovementState == MovementState.InAir && m_Abilities.GetCurrentAbility() == Abilities.PossibleAbilities.Hover)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ChangeState(MovementState.Hovering);
            }
        }

        if (m_MovementState == MovementState.OnGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                m_Velocity.y = JumpStrength;
                m_JumpDelta = 0.0f;
                ChangeState(MovementState.InAir);
                m_IsJumping = true;
            }
        }
    }

    void CalculateEpsilons()
    {
        if (m_Velocity.x >= -MovementEpsilon && m_Velocity.x <= MovementEpsilon)
        {
            m_Velocity.x = 0.0f;
        }

        if (m_Velocity.y >= -VerticalEpsilon && m_Velocity.y <= VerticalEpsilon)
        {
            m_Velocity.y = 0.0f;
        }

        if (m_Velocity.z >= -MovementEpsilon && m_Velocity.z <= MovementEpsilon)
        {
            m_Velocity.z = 0.0f;
        }
    }

    void ChangeState(MovementState newState)
    {
        m_MovementState = newState;

        switch(newState)
        {
            case MovementState.OnGround:
                m_Velocity.y = 0.0f;
                break;

            case MovementState.InAir:
                break;

            case MovementState.Hovering:
                m_Velocity.y = 0.0f;
                break;
        }
    }

    public Vector3 GetVelocity()
    {
        return m_Velocity;
    }

    public Vector3 GetRotationData()
    {
        return new Vector3(PlayerModel.transform.eulerAngles.y, m_RotationDelta / TimeToRotate, m_HasReachedRotation ? 1.0f : 0.0f);
    }

    public Vector2 GetJumpingData()
    {
        return new Vector2(m_JumpDelta / TimeToJump, m_IsJumping ? 1.0f : 0.0f);
    }

    public string GetMovementState()
    {
        return m_MovementState.ToString();
    }

    public void SetLockedMovement(bool flag)
    {
        m_LockMovement = flag;
        m_Velocity = Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2.0f);
    }
}
