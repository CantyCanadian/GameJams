using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushEffect : MonoBehaviour
{
    public BoxCollider Box;
    public float KnockbackFromBadPush = 10.0f;

    private bool m_Push = false;

    public void Push(float pushTime)
    {
        StartCoroutine(PushOrder(pushTime));
    }

    private IEnumerator PushOrder(float swingTime)
    {
        m_Push = true;
        yield return new WaitForSeconds(swingTime);
        m_Push = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Enemy" && m_Push)
        {
            Screenshake.Instance.Shake(0.25f);
            if (other.GetComponent<BaseEnemy>().Push(transform.position))
            {
                Screenshake.Instance.Shake(1.0f);
                m_Push = false;
                StopCoroutine("PushOrder");
                GetComponent<Rigidbody>().AddForce((transform.position - other.transform.position) * KnockbackFromBadPush, ForceMode.VelocityChange);
                GetComponent<BasicMovement>().Stun(1.0f);
            }
        }
    }
}
