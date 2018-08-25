using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingDamage : MonoBehaviour
{
    public BoxCollider Box;

    private bool m_Swing = false;

    public void Swing(float swingTime)
    {
        StartCoroutine(SwingOrder(swingTime));
    }

    private IEnumerator SwingOrder(float swingTime)
    {
        m_Swing = true;
        yield return new WaitForSeconds(swingTime);
        m_Swing = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Enemy" && m_Swing)
        {
            Screenshake.Instance.Shake(0.1f);
            other.GetComponent<BaseEnemy>().Damage(transform.position);
        }
    }
}
