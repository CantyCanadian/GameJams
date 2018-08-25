using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoi : BaseEnemy
{
    public GameObject Gear1;
    public GameObject Gear2;

    public GameObject MatchPivot;

    public float TimeToExplode = 5.0f;

    public float GearRotation;

    private float m_Timer = 1.0f;

    public override IEnumerator EnemyLoop()
    {
        float rotationAngle = 0.0f;
        float explodeDelta = 0.0f;

        while(m_Health > 0)
        {
            rotationAngle = Time.deltaTime * GearRotation * (explodeDelta / TimeToExplode);

            Gear1.transform.Rotate(new Vector3(rotationAngle, 0.0f, 0.0f));
            Gear2.transform.Rotate(new Vector3(rotationAngle, 0.0f, 0.0f));

            explodeDelta += Time.deltaTime;

            m_Timer = explodeDelta / TimeToExplode;

            MatchPivot.transform.localScale = new Vector3(1.0f, Mathf.Lerp(1.0f, 0.0f, m_Timer), 1.0f);

            if (explodeDelta > TimeToExplode)
            {
                OnDeath();
            }

            yield return null;
        }
    }

    public override Vector2 OnDeath()
    {
        MapGenerator.Instance.OnBlockExplosion(transform.position, 3.0f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody>().AddExplosionForce(600.0f, transform.position, 3.0f);
        if (Vector3.Distance(player.transform.position, transform.position) <= 3.0f)
        {
            Screenshake.Instance.Shake(0.5f);
        }
        Destroy(gameObject);

        return new Vector2(0.05f, 0.1f);
    }

    public float GetTimer()
    {
        return m_Timer;
    }
}
