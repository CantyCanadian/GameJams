using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject DrillBoiPrefab;
    public GameObject BombBoiPrefab;

    private float m_Timer = 0.0f;
    private int m_EnemiesPer3Sec = 0;
    private int m_EnemiesLeft = 0;

    private bool m_Start = false;

    public void Initialize()
    {
        m_Start = true;
    }

    public void Stop()
    {
        m_Start = false;
    }

    void Update()
    {
        if (m_Start)
        {
            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0.0f)
            {
                if (m_EnemiesLeft > 0)
                {
                    m_Timer = m_EnemiesPer3Sec / 5.0f;
                    m_EnemiesLeft--;
                    Spawn();
                }
                else
                {
                    m_EnemiesPer3Sec++;
                    m_EnemiesLeft = m_EnemiesPer3Sec - 1;
                    m_Timer = 3.0f / m_EnemiesPer3Sec;
                    Spawn();
                }
            }
        }
	}

    private void Spawn()
    {
        int random = Random.Range(0, 4);

        if (random == 0)
        {
            GameObject block = MapGenerator.Instance.GetRandomBlock();
            GameObject enemy = Instantiate(DrillBoiPrefab);
            enemy.transform.position = new Vector3(block.transform.position.x, 5.0f, block.transform.position.z);
            enemy.GetComponent<BaseEnemy>().Initialize(3.0f);
        }
        else
        {
            GameObject block = MapGenerator.Instance.GetRandomBlock();
            GameObject enemy = Instantiate(BombBoiPrefab);
            enemy.transform.position = new Vector3(block.transform.position.x, 5.0f, block.transform.position.z);
            enemy.GetComponent<BaseEnemy>().Initialize(3.0f);
        }
    }
}
