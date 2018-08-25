using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    public GameObject[] ObjectsToControl;
    public GameObject[] SpawnTeleporters;
    public float[] SpawnRotator;
    public bool[] Location;

    private int m_State = 0;
    private bool m_LookForInput = false;

    void Start()
    {
        foreach (GameObject obj in ObjectsToControl)
        {
            obj.SetActive(false);
        }
    }

	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            switch(m_State)
            {
                case 0:
                    foreach (GameObject obj in ObjectsToControl)
                    {
                        obj.SetActive(!obj.activeSelf);
                    }
                    m_State = 1;
                    break;

                case 1:
                    m_LookForInput = true;
                    m_State = 2;
                    break;

                case 2:
                    m_LookForInput = false;
                    foreach (GameObject obj in ObjectsToControl)
                    {
                        obj.SetActive(!obj.activeSelf);
                    }
                    m_State = 0;
                    break;
            }
        }

        if (m_LookForInput)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[0], SpawnRotator[0], Location[0]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[1], SpawnRotator[1], Location[1]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[2], SpawnRotator[2], Location[2]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[3], SpawnRotator[3], Location[3]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[4], SpawnRotator[4], Location[4]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[5], SpawnRotator[5], Location[5]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[6], SpawnRotator[6], Location[6]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[7], SpawnRotator[7], Location[7]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[8], SpawnRotator[8], Location[8]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                GameManager.Instance.TeleportPlayer(SpawnTeleporters[9], SpawnRotator[9], Location[9]);
            }
        }
	}
}
