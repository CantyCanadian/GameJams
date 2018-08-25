using System;
using UnityEngine;
using System.Collections.Generic;

public class BasicMovement : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public KeyMovementDictionary KeyMovement;
    public float MovementSpeed = 3.0f;

    private float m_Stun = 0.0f;

    private void Update()
    {
        if (m_Stun > 0.0f)
        {
            m_Stun -= Time.deltaTime;
            return;
        }

        Vector2 velocity = new Vector2();

        foreach (KeyValuePair<KeyCode, Direction> kvp in KeyMovement)
        {
            if (Input.GetKey(kvp.Key))
            {
                switch (kvp.Value)
                {
                    case Direction.Up:
                        velocity.y += MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Down:
                        velocity.y -= MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Left:
                        velocity.x -= MovementSpeed * Time.deltaTime;
                        break;

                    case Direction.Right:
                        velocity.x += MovementSpeed * Time.deltaTime;
                        break;
                }
            }
        }

        Vector3 forward = transform.forward * velocity.y;
        Vector3 sideways = transform.right * velocity.x;

        transform.position += forward + sideways;
    }

    public void Stun(float time)
    {
        m_Stun = Mathf.Max(m_Stun, time);
    }

    [System.Serializable] public class KeyMovementDictionary : SerializableDictionary<KeyCode, Direction> { }
}