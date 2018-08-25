using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public GameObject PushObject;

    public GameObject PushPivot1;
    public GameObject PushPivot2;
    public GameObject PushPad;
    public float TimeToPush;
    public float PushDistance;

    private PlayerPushEffect m_Push;

    void Start()
    {
        PushObject.SetActive(false);
        m_Push = GetComponent<PlayerPushEffect>();
        PushPivot1.transform.localPosition = new Vector3();
        PushPivot2.transform.localPosition = new Vector3();
        PushPad.transform.localPosition = new Vector3();
        StartCoroutine(PushLoop());
    }

    private IEnumerator PushLoop()
    {
        bool isPushing = false;
        float pushDelta = 0.0f;

        while(true)
        {
            if (Input.GetMouseButtonDown(1) && !isPushing)
            {
                isPushing = true;
                m_Push.Push(TimeToPush);
                PushObject.SetActive(true);
            }

            if (isPushing)
            {
                pushDelta += Time.deltaTime;

                if (pushDelta < TimeToPush)
                {
                    float fullPosition = Mathf.Lerp(PushDistance, 0.0f, pushDelta / TimeToPush);
                    PushPivot1.transform.localPosition = new Vector3(0.0f, 0.0f, fullPosition / 2.0f);
                    PushPivot2.transform.localPosition = new Vector3(0.0f, 0.0f, fullPosition);
                    PushPad.transform.localPosition = new Vector3(0.0f, 0.0f, fullPosition);
                }
                else
                {
                    PushPivot1.transform.localPosition = new Vector3();
                    PushPivot2.transform.localPosition = new Vector3();
                    PushPad.transform.localPosition = new Vector3();
                    isPushing = false;
                    pushDelta = 0.0f;
                    PushObject.SetActive(false);
                }
            }

            yield return null;
        }
    }
}
