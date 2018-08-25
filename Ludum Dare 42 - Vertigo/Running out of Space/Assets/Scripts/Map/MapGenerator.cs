using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : Singleton<MapGenerator>
{
    public GameObject BlockPrefab;
    public GameObject IndestructiblockPrefab;
    public Vector2Int BlockSize;
    public GameObject Core;

    public MapShape MapType;

    public int HalfWidth;

    public int UnreachablesRecursionByFrame = 1000;

    private List<GameObject> m_BlockList = null;
    private Dictionary<GameObject, Vector2Int> m_Positions = null;
    private Dictionary<GameObject, bool> m_Destructible = null;
    private List<List<NavMeshSurface>> m_MeshArray = null;

    private int MiddleHP = 5;

    public enum MapShape
    {
        Circle,
        Square,
        Diamond
    }

    public bool OnBlockDestroy(GameObject block, bool refreshMesh = true)
    {
        if (!m_Destructible[block])
        {
            if (block.transform.position == new Vector3())
            {
                MiddleHP--;

                switch(MiddleHP)
                {
                    case 4:
                        block.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 0.25f);
                        break;

                    case 3:
                        block.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 0.50f);
                        break;

                    case 2:
                        block.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 0.75f);
                        break;

                    case 1:
                        block.GetComponent<Renderer>().material.color = Color.black;
                        break;

                    default:
                        for(int i = 0; i < m_BlockList.Count; i++)
                        {
                            Destroy(m_BlockList[i]);
                        }
                        m_BlockList.Clear();
                        break;
                }
            }

            return false;
        }

        m_BlockList.Remove(block);
        Destroy(block);

        //if (refreshMesh)
        //{
        //    RefreshNavMesh(m_Positions[block]);
        //}

        return true;
    }

    public void OnBlockExplosion(Vector3 position, float power)
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for(int i = 0; i < m_BlockList.Count; i++)
        {
            if (Mathf.Abs(Vector3.Distance(position, m_BlockList[i].transform.position)) <= power)
            {
                toDestroy.Add(m_BlockList[i]);
            }
        }

        for(int i = 0; i < toDestroy.Count; i++)
        {
            OnBlockDestroy(toDestroy[i], false);
        }

        //for (int i = 0; i < m_BlockList.Count; i++)
        //{
        //    if (Mathf.Abs(Vector3.Distance(position, m_BlockList[i].transform.position)) <= power + 1.0f)
        //    {
        //        m_BlockList[i].GetComponent<NavMeshSurface>().BuildNavMesh();
        //    }
        //}
    }

    public GameObject GetRandomBlock()
    {
        return m_BlockList == null ? null : m_BlockList.GetRandom();
    }

    private void RefreshNavMesh()
    {
        for (int i = 0; i < m_BlockList.Count; i++)
        {
            m_BlockList[i].GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    private void RefreshNavMesh(Vector2Int position)
    {
        if (position.x - 1 > 0)
        {
            NavMeshSurface nms = m_MeshArray[position.x - 1][position.y];

            if (nms != null)
            {
                nms.BuildNavMesh();
            }
        }
        
        if (position.x + 1 < m_MeshArray.Count)
        {
            NavMeshSurface nms = m_MeshArray[position.x + 1][position.y];

            if (nms != null)
            {
                nms.BuildNavMesh();
            }
        }

        if (position.y - 1 > 0)
        {
            NavMeshSurface nms = m_MeshArray[position.x][position.y - 1];

            if (nms != null)
            {
                nms.BuildNavMesh();
            }
        }

        if (position.y + 1 < m_MeshArray[position.x].Count)
        {
            NavMeshSurface nms = m_MeshArray[position.x][position.y + 1];

            if (nms != null)
            {
                nms.BuildNavMesh();
            }
        }
    }

    private void Start ()
    {
        m_MeshArray = new List<List<NavMeshSurface>>();
        m_Positions = new Dictionary<GameObject, Vector2Int>();
        m_BlockList = new List<GameObject>();
        m_Destructible = new Dictionary<GameObject, bool>();

		switch(MapType)
        {
            case MapShape.Circle:
                {
                    for (int i = 0; i < HalfWidth * 2 + 1; i++)
                    {
                        int x = i - HalfWidth;
                        m_MeshArray.Add(new List<NavMeshSurface>());

                        for (int j = 0; j < HalfWidth * 2 + 1; j++)
                        {
                            if (i == HalfWidth && j == HalfWidth)
                            {
                                continue;
                            }

                            int y = j - HalfWidth;

                            Vector3 position = new Vector3(BlockSize.x * x, 0.0f, BlockSize.y * y);

                            if (Mathf.Abs(Vector3.Distance(transform.position, position)) <= HalfWidth)
                            {
                                GameObject block = Instantiate(BlockPrefab, transform);
                                block.name = (x + y).ToString();
                                block.transform.position = position;

                                m_MeshArray[i].Add(block.GetComponent<NavMeshSurface>());
                                m_BlockList.Add(block);
                                m_Positions.Add(block, new Vector2Int(i, j));
                            }
                            else
                            {
                                m_MeshArray[i].Add(null);
                            }
                        }
                    }
                }
                break;

            case MapShape.Square:
                {
                    for (int i = 0; i < HalfWidth * 2 + 1; i++)
                    {
                        int x = i - HalfWidth;
                        m_MeshArray.Add(new List<NavMeshSurface>());

                        for (int j = 0; j < HalfWidth * 2 + 1; j++)
                        {
                            if (i == HalfWidth && j == HalfWidth)
                            {
                                m_Destructible.Add(Core, false);
                                m_BlockList.Add(Core);
                                m_MeshArray[i].Add(Core.GetComponent<NavMeshSurface>());
                                m_Positions.Add(Core, new Vector2Int(i, j));
                                continue;
                            }

                            int y = j - HalfWidth;

                            Vector3 position = new Vector3(BlockSize.x * x, 0.0f, BlockSize.y * y);
                            GameObject block = null;

                            if (position.magnitude > 4.0f)
                            {
                                block = Instantiate(BlockPrefab, transform);
                                m_Destructible.Add(block, true);
                            }
                            else
                            {
                                block = Instantiate(IndestructiblockPrefab, transform);
                                m_Destructible.Add(block, false);
                            }

                            block.transform.position = position;
                            m_BlockList.Add(block);
                            m_MeshArray[i].Add(block.GetComponent<NavMeshSurface>());
                            m_Positions.Add(block, new Vector2Int(i, j));
                        }
                    }
                }
                break;

            case MapShape.Diamond:
                {
                    for (int i = 0; i < HalfWidth * 2 + 1; i++)
                    {
                        int x = i - HalfWidth;
                        m_MeshArray.Add(new List<NavMeshSurface>());

                        for (int j = 0; j < HalfWidth * 2 + 1; j++)
                        {
                            if (i == HalfWidth && j == HalfWidth)
                            {
                                continue;
                            }

                            int y = j - HalfWidth;

                            Vector3 position = new Vector3(BlockSize.x * x, 0.0f, BlockSize.y * y);

                            if (Mathf.Abs(x) + Mathf.Abs(y) <= HalfWidth)
                            {
                                GameObject block = Instantiate(BlockPrefab, transform);
                                block.transform.position = position;

                                m_MeshArray[i].Add(block.GetComponent<NavMeshSurface>());
                                m_BlockList.Add(block);
                                m_Positions.Add(block, new Vector2Int(i, j));
                            }
                            else
                            {
                                m_MeshArray[i].Add(null);
                            }
                        }
                    }
                }
                break;
        }

        RefreshNavMesh();
	}
}
