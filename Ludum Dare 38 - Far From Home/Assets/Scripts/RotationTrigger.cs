using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrigger : MonoBehaviour
{
    public GameObject rotator;
    public float Angle;
    public bool ApplyNewPoint = true;
    public GameObject NewRespawnPoint;
    public bool IsInside = false;

    private bool m_Invert = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            float newAngle = m_Invert ? -Angle : Angle;
            m_Invert = !m_Invert;

            GameManager.Instance.InitiateRotation(newAngle, transform, IsInside);

            if (ApplyNewPoint)
            {
                GameManager.Instance.SetRespawnPoint(NewRespawnPoint.transform, rotator.transform.position.y + newAngle, IsInside);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
