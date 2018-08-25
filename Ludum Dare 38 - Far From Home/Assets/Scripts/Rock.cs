using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float Speed = 10.0f;
    public float TimeAlive = 5.0f;

    private Vector3 m_Direction;
    private bool m_Active = false;

	void Update ()
    {
		if (m_Active)
        {
            Vector3 oldPosition = transform.position;
            transform.position += m_Direction * Speed * Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(oldPosition, m_Direction, out hit, Vector3.Distance(oldPosition, transform.position), ~LayerMask.GetMask("Projectile", "Player")))
            {
                Debug.Log("Hit " + hit.transform.name);

                if(hit.transform.tag == "Target")
                {
                    hit.transform.GetComponent<Target>().Activate();
                }

                Destroy(gameObject);
            }
        }
	}

    public void Activate(Vector3 direction)
    {
        Destroy(gameObject, TimeAlive);
        m_Active = true;
        m_Direction = direction;
    }
}
