using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameLinker : MonoBehaviour
{
    public GameObject LoadGameObject;

    public GameObject LoadGamePrefab;
    public GameObject LoadGameFiller;
    public GameObject[] BoxParentArray;
    public GameObject[] BoxSizeArray;

    private List<string> m_SavesLocations;
    private List<LoadGameBox> m_LoadGameBoxes;
    private List<GameObject> m_Fillers;

    private bool m_Move = false;
    private bool m_Back = false;
    private int m_Index = 0;

    private string m_Result = string.Empty;

    public void Initialize()
    {
        m_SavesLocations = new List<string>();
        m_LoadGameBoxes = new List<LoadGameBox>();
        m_Fillers = new List<GameObject>();

        LoadGameObject.SetActive(false);

        RefreshSaveBoxes();
    }

    public string GetResult()
    {
        return m_Result;
    }

    public void OnUpPressed()
    {
        if (m_Index - 1 < 0)
        {
            return;
        }

        m_Index--;
        m_Move = true;
    }

    public void OnDownPressed()
    {
        if (m_Index + 1 >= m_LoadGameBoxes.Count)
        {
            return;
        }

        m_Index++;
        m_Move = true;
    }

    public void OnLoadPressed()
    {
        if (m_LoadGameBoxes.Count == 0)
        {
            return;
        }

        m_Result = m_SavesLocations[m_Index];
        m_Back = true;
    }

    public void OnBackPressed()
    {
        m_Result = string.Empty;
        m_Back = true;
    }

    public IEnumerator LoadGameLoop()
    {
        m_LoadGameBoxes.DoOnAll(new System.Func<LoadGameBox, LoadGameBox>((obj1) => { obj1.LoadData(); return obj1; }));

        Coroutine input = StartCoroutine(LoadGameInput());

        m_Index = 0;
        SetSaveBoxes(m_Index);

        bool first = true;
        LoadGameObject.SetActive(true);

        GameManager.Instance.Background.ShowScreenBackground();

        yield return StartCoroutine(GameManager.Instance.Transition.FadeOut());

        m_Move = false;

        while (m_Back == false)
        {
            if (first == true)
            {
                first = false;
            }
            else
            {
                SetSaveBoxes(m_Index);
            }
            
            yield return new WaitUntil(() => m_Move || m_Back);
            m_Move = false;
        }
        
        m_Back = false;

        yield return StartCoroutine(GameManager.Instance.Transition.FadeIn());

        LoadGameObject.SetActive(false);

        StopCoroutine(input);
    }

    public IEnumerator LoadGameInput()
    {
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnUpPressed();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnDownPressed();
            }

            if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f)
            {
                OnUpPressed();
            }

            if (Input.GetAxis("Mouse ScrollWheel") <= -0.1f)
            {
                OnDownPressed();
            }

            yield return null;
        }
    }

    private void SetSaveBoxes(int middleIndex)
    {
        for(int i = 0; i < m_Fillers.Count; i++)
        {
            Destroy(m_Fillers[i]);
        }

        m_Fillers.Clear();

        m_LoadGameBoxes.DoOnAll(new System.Func<LoadGameBox, LoadGameBox>((obj1) => { obj1.gameObject.SetActive(false); return obj1; }));

        int count = m_LoadGameBoxes.Count;

        if (m_LoadGameBoxes.Count > 0)
        {
            m_LoadGameBoxes[middleIndex].gameObject.SetActive(true);
            m_LoadGameBoxes[middleIndex].SetTransform(BoxSizeArray[2], BoxParentArray[2]);
        }
        else
        {
            GameObject boxMid = Instantiate(LoadGameFiller);
            boxMid.transform.SetParent(BoxParentArray[2].transform);
            boxMid.transform.localPosition = new Vector3();
            m_Fillers.Add(boxMid);
        }
        
        if (middleIndex - 1 >= 0)
        {
            m_LoadGameBoxes[middleIndex - 1].gameObject.SetActive(true);
            m_LoadGameBoxes[middleIndex - 1].SetTransform(BoxSizeArray[1], BoxParentArray[1]);
        }
        else
        {
            GameObject boxMinus1 = Instantiate(LoadGameFiller);
            boxMinus1.transform.SetParent(BoxParentArray[1].transform);
            boxMinus1.transform.localPosition = new Vector3();
            boxMinus1.transform.localScale = BoxSizeArray[1].transform.localScale;
            m_Fillers.Add(boxMinus1);
        }

        if (middleIndex - 2 >= 0)
        {
            m_LoadGameBoxes[middleIndex - 2].gameObject.SetActive(true);
            m_LoadGameBoxes[middleIndex - 2].SetTransform(BoxSizeArray[0], BoxParentArray[0]);
        }
        else
        {
            GameObject boxMinus2 = Instantiate(LoadGameFiller);
            boxMinus2.transform.SetParent(BoxParentArray[0].transform);
            boxMinus2.transform.localPosition = new Vector3();
            boxMinus2.transform.localScale = BoxSizeArray[0].transform.localScale;
            m_Fillers.Add(boxMinus2);
        }

        if (middleIndex + 1 < m_LoadGameBoxes.Count)
        {
            m_LoadGameBoxes[middleIndex + 1].gameObject.SetActive(true);
            m_LoadGameBoxes[middleIndex + 1].SetTransform(BoxSizeArray[3], BoxParentArray[3]);
        }
        else
        {
            GameObject boxPlus1 = Instantiate(LoadGameFiller);
            boxPlus1.transform.SetParent(BoxParentArray[3].transform);
            boxPlus1.transform.localPosition = new Vector3();
            boxPlus1.transform.localScale = BoxSizeArray[3].transform.localScale;
            m_Fillers.Add(boxPlus1);
        }

        if (middleIndex + 2 < m_LoadGameBoxes.Count)
        {
            m_LoadGameBoxes[middleIndex + 2].gameObject.SetActive(true);
            m_LoadGameBoxes[middleIndex + 2].SetTransform(BoxSizeArray[4], BoxParentArray[4]);
        }
        else
        {
            GameObject boxPlus2 = Instantiate(LoadGameFiller);
            boxPlus2.transform.SetParent(BoxParentArray[4].transform);
            boxPlus2.transform.localPosition = new Vector3();
            boxPlus2.transform.localScale = BoxSizeArray[4].transform.localScale;
            m_Fillers.Add(boxPlus2);
        }
    }

    private void RefreshSaveBoxes()
    {
        for(int i = 0; i < m_LoadGameBoxes.Count; i++)
        {
            Destroy(m_LoadGameBoxes[i]);
        }

        m_LoadGameBoxes.Clear();

        string directory = GameManager.Instance.Saving.Directory;
        string[] allFiles = System.IO.Directory.GetFiles(directory, "*.sav");

        foreach (string file in allFiles)
        {
            GameObject box = Instantiate(LoadGamePrefab);

            string[] loadScreenData = GameManager.Instance.Saving.ExtractLoadScreenData(file);

            if (loadScreenData == null)
            {
                continue;
            }

            LoadGameBox lgb = box.GetComponent<LoadGameBox>();
            lgb.SetData(loadScreenData[1], int.Parse(loadScreenData[2]), int.Parse(loadScreenData[3]), int.Parse(loadScreenData[4]));

            m_LoadGameBoxes.Add(lgb);
            m_SavesLocations.Add(file);
        }
    }
}
