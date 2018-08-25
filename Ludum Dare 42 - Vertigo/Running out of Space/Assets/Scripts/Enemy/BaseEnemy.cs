using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    public GameObject HealthBar;
    public int Health;

    public bool OnDeathOnKill = false;

    public float InvulnerabilityTime = 0.25f;
    public float KnockbackFromDamage = 10.0f;
    public float KnockbackFromPush = 2.0f;
    public bool CanBePushed = true;

    protected int m_Health;

    private bool m_Damaged = false;
    private bool m_Pushed = false;

    private float m_TimeToStart = 0.0f;

    public void Damage(Vector3 source)
    {
        if (m_Damaged)
        {
            return;
        }

        m_Damaged = true;
        m_Health--;

        if (m_Health <= 0)
        {
            if (OnDeathOnKill)
            {
                OnDeath();
            }

            Destroy(gameObject);
        }
    }

    public bool Push(Vector3 source)
    {
        if (m_Pushed)
        {
            return false;
        }
        else if (!CanBePushed)
        {
            return true;
        }

        m_Pushed = true;
        GetComponent<Rigidbody>().AddForce((transform.position - source) * KnockbackFromPush, ForceMode.VelocityChange);

        return false;
    }

    public int GetHealth()
    {
        return m_Health;
    }

    public void Initialize(float timeToStart)
    {
        m_Health = Health;
        StartCoroutine(EnemyLoop());
        StartCoroutine(DamageLoop());
        StartCoroutine(PushLoop());
        StartCoroutine(NavMeshLoop());
        m_TimeToStart = timeToStart;
    }

    private IEnumerator DamageLoop()
    {
        while (true)
        {
            if (m_Damaged)
            {
                yield return new WaitForSeconds(InvulnerabilityTime);
                m_Damaged = false;
            }

            yield return null;
        }
    }

    private IEnumerator PushLoop()
    {
        while (true)
        {
            if (m_Pushed)
            {
                yield return new WaitForSeconds(InvulnerabilityTime);
                m_Pushed = false;
            }

            yield return null;
        }
    }

    private IEnumerator NavMeshLoop()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            bool pathSet = false;
            bool pathDone = false;

            GameObject blockBuffer = null;

            yield return new WaitForSeconds(m_TimeToStart);
            agent.enabled = true;

            while (true)
            {
                if (agent.enabled)
                {
                    bool floor = false;
                    RaycastHit[] under = Physics.RaycastAll(transform.position, -transform.up, 10.0f);
                    foreach(RaycastHit rh in under)
                    {
                        if (rh.transform.tag == "Floor")
                        {
                            floor = true;
                            break;
                        }
                    }

                    if (floor == false)
                    {
                        agent.enabled = false;
                        continue;
                    }

                    if (!pathSet)
                    {
                        blockBuffer = MapGenerator.Instance.GetRandomBlock();

                        if (blockBuffer == null)
                        {
                            yield return null;
                            continue;
                        }

                        Vector3 target = new Vector3(blockBuffer.transform.position.x, transform.position.y, blockBuffer.transform.position.z);

                        agent.SetDestination(target);

                        pathSet = true;
                    }
                    else if (!pathDone)
                    {
                        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
                        {
                            pathSet = false;
                        }

                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            pathDone = true;
                        }

                        yield return null;
                    }
                    else
                    {
                        m_Damaged = true;
                        m_Pushed = true;
                        agent.enabled = false;

                        Vector2 time = OnDeath();

                        yield return new WaitForSeconds(time.x);

                        MapGenerator.Instance.OnBlockDestroy(blockBuffer);

                        yield return new WaitForSeconds(time.y - time.x);

                        Destroy(gameObject);
                    }
                }

                yield return null;
            }
        }
    }

    public abstract IEnumerator EnemyLoop();

    public abstract Vector2 OnDeath();
}
