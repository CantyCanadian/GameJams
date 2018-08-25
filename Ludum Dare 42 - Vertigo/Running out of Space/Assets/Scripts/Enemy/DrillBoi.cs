using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBoi : BaseEnemy
{
    public GameObject Gear1;
    public GameObject Gear2;

    public float GearRotation;

    public float DrillAnimation;

    private bool m_Drill = false;

    private Vector3 m_TempPosition;

    public override IEnumerator EnemyLoop()
    {
        float rotationAngle = 0.0f;
        float drillAnimation = 0.0f;

        while(m_Health > 0)
        {
            rotationAngle = Time.deltaTime * GearRotation;

            Gear1.transform.Rotate(new Vector3(0.0f, rotationAngle, 0.0f));
            Gear2.transform.Rotate(new Vector3(rotationAngle, 0.0f, 0.0f));

            if (m_Drill)
            {
                drillAnimation += Time.deltaTime;

                float halfAnimation = DrillAnimation / 2.0f;

                if (drillAnimation <= halfAnimation)
                {
                    float rotation = Mathf.Lerp(0.0f, 180.0f, drillAnimation / halfAnimation);

                    float newPosition = Mathf.Lerp(m_TempPosition.y, 2.0f, drillAnimation / halfAnimation);

                    transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

                    transform.eulerAngles = new Vector3(rotation, 0.0f, 0.0f);
                }
                else
                {
                    float newDelta = drillAnimation - halfAnimation;

                    float newPosition = Mathf.Lerp(2.0f, -9.0f, newDelta / halfAnimation);

                    transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
                }
            }

            yield return null;
        }
    }

    public override Vector2 OnDeath()
    {
        m_Drill = true;
        m_TempPosition = transform.position;
        return new Vector2(DrillAnimation * 0.5f, DrillAnimation);
    }
}
