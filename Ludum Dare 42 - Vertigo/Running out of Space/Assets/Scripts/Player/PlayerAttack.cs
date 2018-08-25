using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject AttackObject;

    public GameObject SwordPivot;
    public float TimeToSwing;
    public float SwingArc;
    public float RestingAngle;

    private PlayerSwingDamage m_Swing;

    void Start()
    {
        AttackObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        m_Swing = GetComponent<PlayerSwingDamage>();
        SwordPivot.transform.eulerAngles = new Vector3(0.0f, RestingAngle, 0.0f);
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        bool isAttacking = false;
        float swingDelta = 0.0f;

        while(true)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                AttackObject.SetActive(true);
                isAttacking = true;
                m_Swing.Swing(TimeToSwing);
            }

            if (isAttacking)
            {
                swingDelta += Time.deltaTime;

                if (swingDelta < TimeToSwing)
                {
                    float rotation = Mathf.Lerp(0.0f, SwingArc, swingDelta / TimeToSwing);
                    SwordPivot.transform.localEulerAngles = new Vector3(0.0f, rotation - (SwingArc / 2.0f), 0.0f);
                }
                else
                {
                    SwordPivot.transform.localEulerAngles = new Vector3(0.0f, RestingAngle, 0.0f);
                    isAttacking = false;
                    swingDelta = 0.0f;
                    AttackObject.SetActive(false);
                }
            }

            yield return null;
        }
    }
}
